using FMODUnity;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;


namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        FootStepAudioController footStepAudioController;
        
        
        Camera m_Camera;
        CharacterController m_CharacterController;
        CollisionFlags m_CollisionFlags;


        [SerializeField] FOVKick m_FovKick = new FOVKick();
        [SerializeField] float m_GravityMultiplier;
        [SerializeField] CurveControlledBob m_HeadBob = new CurveControlledBob();
        Vector2 m_Input;
        [SerializeField] bool m_IsWalking;
        bool m_Jump;
        [SerializeField] LerpControlledBob m_JumpBob = new LerpControlledBob();
        bool m_Jumping;
        [SerializeField] float m_JumpSpeed;
        [SerializeField] MouseLook m_MouseLook;
        Vector3 m_MoveDir = Vector3.zero;
        float m_NextStep;
        Vector3 m_OriginalCameraPosition;
        bool m_PreviouslyGrounded;
        [SerializeField] float m_RunSpeed;
        [SerializeField] [Range(0f, 1f)] float m_RunstepLenghten;
        float m_StepCycle;
        [SerializeField] float m_StepInterval;
        [SerializeField] float m_StickToGroundForce;
        [SerializeField] bool m_UseFovKick;
        [SerializeField] bool m_UseHeadBob;
        [SerializeField] float m_WalkSpeed;

        float m_YRotation;

        // Use this for initialization
        void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle / 2f;
            m_Jumping = false;
            m_MouseLook.Init(transform, m_Camera.transform);

            footStepAudioController = GetComponent<FootStepAudioController>();

        }


        // Update is called once per frame
        void Update()
        {
            RotateView();
            // the jump state needs to read here to make sure it is not missed
            if (!m_Jump) m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }

            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded) m_MoveDir.y = 0f;

            m_PreviouslyGrounded = m_CharacterController.isGrounded;
        }


        void PlayLandingSound()
        {
            // m_AudioSource.clip = m_LandSound;
            // m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }


        void FixedUpdate()
        {
            float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            var desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x * speed;
            m_MoveDir.z = desiredMove.z * speed;


            if (m_CharacterController.isGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;

                if (m_Jump)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    PlayJumpSound();
                    m_Jump = false;
                    m_Jumping = true;
                }
            }
            else
            {
                m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
            }

            m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

            m_MouseLook.UpdateCursorLock();
        }


        void PlayJumpSound()
        {
            footStepAudioController.PlayJumpSound();
        }


        void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
                m_StepCycle += (m_CharacterController.velocity.magnitude +
                                speed * (m_IsWalking ? 1f : m_RunstepLenghten)) *
                               Time.fixedDeltaTime;

            if (!(m_StepCycle > m_NextStep)) return;

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded) return;
            footStepAudioController.PlayFootStep();
        }


        void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob) return;
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                        speed * (m_IsWalking ? 1f : m_RunstepLenghten));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }

            m_Camera.transform.localPosition = newCameraPosition;
        }


        void GetInput(out float speed)
        {
            // Read input
            var horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            var vertical = CrossPlatformInputManager.GetAxis("Vertical");

            var waswalking = m_IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1) m_Input.Normalize();

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }


        void RotateView()
        {
            m_MouseLook.LookRotation(transform, m_Camera.transform);
        }


        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            var body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below) return;

            if (body == null || body.isKinematic) return;
            body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
        }
    }
}