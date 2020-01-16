using UnityEngine;
using UnityEngine.UI;

public class Player : Entity
{
    public enum Class
    {
        Mage,
        Knight,
        Archer
    }

    [Space]
    public GameObject[] characterList;
    public static Class pClass;
    public GameObject interact;
    public GameObject bowPower;

    private Rigidbody rb;
    private Transform t;
    private Animator a;
    public PauseMenu pm;

    public GameObject inv;
    public GameObject interactInv;
    public GameObject equipment;
    [Space]
    public AudioSource hit;
    public AudioSource archerShoot;
    public AudioSource knightSlash;
    public AudioSource knightBlock;
    public AudioSource miss;
    [Space]
    public InventoryManager manager;

    //interactable in range
    private bool interInRange = false;
    private Loot interactable;
    private bool interactActive = false;

    bool moveForward = false;
    bool moveBackward = false;
    bool moveLeft = false;
    bool moveRight = false;
    bool jump = false;
    bool sprint = false;
    bool crawl = false;
    bool isGrounded = true;
    public bool rClickHeld = false;

    //Speeds
    const float baseSpeed = 50f;
    const float jumpHeight = 500f;
    const float drag = 10f;
    const float friction = 30f;
    //maxes
    const float baseMaxSpeed = 10f;
    const float maxJumpSpeed = 8f;
    //modifiers
    const float airSpeed = 3f;
    const float crawlSpeed = -6f;
    const float sprintSpeed = 6f;
    const float rClickSpeed = -3.5f;

    //options
    int mouseSens = 100;
    bool invertY = false;

    float distToGround;


    public Texture2D crosshairTexture;
    private float crosshairScale = 1;
    void OnGUI()
    {
        //if not paused
        if (Time.timeScale != 0 && !inv.activeSelf)
        {
            if (crosshairTexture != null)
            {
                GUI.DrawTexture(new Rect((Screen.width - crosshairTexture.width * crosshairScale) / 2, (Screen.height - crosshairTexture.height * crosshairScale) / 2, crosshairTexture.width * crosshairScale, crosshairTexture.height * crosshairScale), crosshairTexture);
            }
            else
            {
                Debug.Log("No crosshair texture set in the Inspector");
            }
            interact.SetActive(interInRange);
        }
        else
        {
            interact.SetActive(false);
        }
    }

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        distToGround = GetComponent<Collider>().bounds.extents.y;

        //gravity
        Physics.gravity = new Vector3(0, -15, 0);

        //setup player
        pClass = (Class)PlayerPrefs.GetInt("CharacterSelected");
        health = 20;
        maxHealth = 20;
        baseAttack = 1;
        attack = baseAttack;

        //Fill array with the character models
        for (int i = 0; i < transform.childCount; i++)
        {
            //Disable visibility
            characterList[i].SetActive(false);
        }

        if (characterList[(int)pClass])
        {
            characterList[(int)pClass].SetActive(true);
            t = characterList[(int)pClass].transform;
            a = characterList[(int)pClass].GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        interInRange = InteractableInRange();

        //(un)pause game
        if (Input.GetKeyDown(KeyCode.Escape) && !inv.activeSelf)
        {
            pm.Pause();
        }

        //inv
        if (Input.GetKeyDown(KeyCode.E) || (Input.GetKeyDown(KeyCode.Escape) && inv.activeSelf))
        {
            manager.HideTooltip(null);
            //interaction
            if (interInRange)
            {
                if (interactInv)
                {
                    Destroy(interactInv);
                }
                interactActive = !interactActive;
                if (interactActive)
                {
                    Inventory actual = interactable.gameObject.GetComponentInChildren<Inventory>();
                    interactInv = Instantiate(actual.gameObject);
                    interactInv.transform.SetParent(inv.transform);
                    interactInv.transform.localPosition = new Vector3(0, 255, 0);
                    interactInv.transform.localScale = new Vector3(1, 1, 1);

                    manager.FetchChestInv(interactInv.GetComponent<Inventory>(), actual);
                }
                equipment.SetActive(!equipment.activeSelf);
                interactInv.SetActive(interactActive);
            }
            else if (inv.activeSelf)
            {
                equipment.SetActive(true);
                if (interactInv)
                {
                    interactInv.SetActive(false);
                }
            }
            //toggle inv
            inv.SetActive(!inv.activeSelf);

            if(inv.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        

        if (Time.timeScale != 0)
        {
            if (!inv.activeSelf)
            {
                //Controller Input. Not sure how it'll be used
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");

                //get mouse input for camera
                float invert = invertY ? 1f : -1f;
                float yRot = invert * Input.GetAxis("Mouse Y") * (mouseSens / 100f);
                float xRot = Input.GetAxis("Mouse X") * (mouseSens / 100f);

                //360 max
                Vector3 angle = cam.transform.localEulerAngles;

                //limit camera from going too high/low (so user can't go upside down)
                float angY = angle.x + yRot;
                float angX = angle.y + xRot;
                if (angY >= 90 && angY < 250 && yRot > 0)
                {
                    cam.transform.localEulerAngles = new Vector3(90f, angX, 0);
                }
                else if (angY < 280 && angY > 200)
                {
                    cam.transform.localEulerAngles = new Vector3(280f, angX, 0);
                }
                else
                {
                    //rotate camera x: up, y: right, z: tilt
                    //mouse's y is character's x, mouse's x is character's y
                    cam.transform.localEulerAngles = new Vector3(angY, angX, 0);
                }

                //Combat
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    a.SetTrigger("Attack1Trigger");
                }
                else if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    a.SetTrigger("Attack2");
                }
                //if right click held
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    rClickHeld = true;
                }
                else
                {
                    rClickHeld = false;
                }
                if (pClass == Class.Archer)
                {
                    a.SetBool("Drawing", rClickHeld);
                }
                else if (pClass == Class.Knight)
                {
                    a.SetBool("Blocking", rClickHeld);
                }

                //User directional input
                a.SetBool("Moving", false);
                if (Input.GetKey("a"))
                {
                    a.SetBool("Moving", true);
                    moveLeft = true;
                }
                if (Input.GetKey("s"))
                {
                    a.SetBool("Moving", true);
                    moveBackward = true;
                }
                if (Input.GetKey("w"))
                {
                    a.SetBool("Moving", true);
                    moveForward = true;
                }
                if (Input.GetKey("d"))
                {
                    a.SetBool("Moving", true);
                    moveRight = true;
                }
                if (Input.GetKeyDown(KeyCode.Space) || (Input.GetKey(KeyCode.Space) && isGrounded && !jump))
                {
                    jump = true;
                }
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    sprint = true;
                }
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    if (!a.GetBool("IsCrawling"))
                    {
                        a.SetTrigger("EnterCrawl");
                    }
                    crawl = true;
                    sprint = false;
                }
                else
                {
                    if (a.GetBool("IsCrawling"))
                    {
                        a.SetTrigger("ExitCrawl");
                    }
                }
                if (moveBackward == moveForward && moveLeft == moveRight)
                {
                    a.SetBool("Moving", false);
                }
                a.SetFloat("Input X", v);
                a.SetFloat("Input Z", h);
            }

            //Move the camera to the new position of the body
            Vector3 locPos = t.InverseTransformDirection(rb.position);
            float y = crawl ? locPos.y + 0.8F : locPos.y + 1;
            cam.transform.position = t.TransformDirection(new Vector3(locPos.x, y, locPos.z + 0.4F));
        }

        //kill the player if it falls out of the world
        if (transform.position.y <= 0)
        {
            TakeDamage(float.MaxValue);
        }
    }

    void FixedUpdate()
    {
        if (Time.timeScale != 0)
        {
            //Move avatar to rigidbody location
            t.position = new Vector3(rb.position.x, rb.position.y - 1F, rb.position.z);
            //Rotate the body to face the same direction (left & right) as the camera
            rb.transform.localRotation = new Quaternion(0, cam.transform.rotation.y, 0, cam.transform.rotation.w);
            t.rotation = rb.rotation;
            //local velocity so that velocity is changed based on facing direction
            Vector3 locVel = transform.InverseTransformDirection(rb.velocity);

            float currMaxSpeed = baseMaxSpeed + (baseSpeed * (agility * 0.01f));
            float currSpeed = baseSpeed + (baseSpeed * (agility * 0.01f));
            if (!isGrounded)
            {
                currMaxSpeed += airSpeed;
            }
            if (sprint)
            {
                currMaxSpeed += sprintSpeed;
                currSpeed += sprintSpeed;
                sprint = false;
            }
            if (crawl)
            {
                currMaxSpeed += crawlSpeed;
                currSpeed += crawlSpeed;
                crawl = false;
            }
            if (rClickHeld)
            {
                currMaxSpeed += rClickSpeed;
                currSpeed += rClickSpeed;
            }

            if (moveRight)
            {
                locVel.x += currSpeed * Time.fixedDeltaTime;
            }
            if (moveLeft)
            {
                locVel.x -= currSpeed * Time.fixedDeltaTime;
            }
            if (moveForward)
            {
                locVel.z += currSpeed * Time.fixedDeltaTime;
            }
            if (moveBackward)
            {
                locVel.z -= currSpeed * Time.fixedDeltaTime;
            }
            if (jump && isGrounded && CloseToGround())
            {
                locVel.y += jumpHeight * Time.fixedDeltaTime;
            }

            float speedReduction = isGrounded ? friction : drag;
            //checking if neither/both directions for the axis is pressed, then slowly brings velocity down to 0 (if true)
            if (moveLeft == moveRight)
            {
                if (locVel.x > 0)
                {
                    locVel.x -= speedReduction * Time.fixedDeltaTime;
                    if (locVel.x < 0)
                    {
                        locVel.x = 0;
                    }
                }
                else if (locVel.x < 0)
                {
                    locVel.x += speedReduction * Time.fixedDeltaTime;
                    if (locVel.x > 0)
                    {
                        locVel.x = 0;
                    }
                }
            }
            if (moveForward == moveBackward)
            {
                if (locVel.z > 0)
                {
                    locVel.z -= speedReduction * Time.fixedDeltaTime;
                    if (locVel.z < 0)
                    {
                        locVel.z = 0;
                    }
                }
                else if (locVel.z < 0)
                {
                    locVel.z += speedReduction * Time.fixedDeltaTime;
                    if (locVel.z > 0)
                    {
                        locVel.z = 0;
                    }
                }
            }

            //limit speed
            if (locVel.x > currMaxSpeed)
            {
                locVel = new Vector3(currMaxSpeed, locVel.y, locVel.z);
            }
            if (locVel.z > currMaxSpeed)
            {
                locVel = new Vector3(locVel.x, locVel.y, currMaxSpeed);
            }
            if (locVel.x < -currMaxSpeed)
            {
                locVel = new Vector3(-currMaxSpeed, locVel.y, locVel.z);
            }
            if (locVel.z < -currMaxSpeed)
            {
                locVel = new Vector3(locVel.x, locVel.y, -currMaxSpeed);
            }
            if (locVel.y > maxJumpSpeed)
            {
                locVel = new Vector3(locVel.x, maxJumpSpeed, locVel.z);
            }

            //convert the local velocity into world velocity and add it to the rigidbody
            rb.velocity = transform.TransformDirection(locVel);

            //reset vars
            moveRight = false;
            moveLeft = false;
            moveForward = false;
            moveBackward = false;
            jump = false;
        }
    }

    bool CloseToGround()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGround + 0.5f);
    }

    bool InteractableInRange()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)), out hit, 3f))
        {
            bool isInter = hit.collider.tag == "Interactable";
            if (isInter)
            {
                interactable = hit.collider.gameObject.GetComponentInParent<Loot>();
            }
            return isInter;
        }
        else
        {
            return false;
        }

    }

    //Animation Receiver
    public void FootStep()
    {

    }

    void OnCollisionEnter(Collision theCollision)
    {
        //Layer 8 is terrain
        if (theCollision.gameObject.layer == 8)
        {
            isGrounded = true;
        }
    }

    //called when player leaves ground collisoin
    void OnCollisionExit(Collision theCollision)
    {
        //Layer 8 is terrain
        if (theCollision.gameObject.layer == 8)
        {
            isGrounded = false;
        }
    }

    //Sets mouse sensetivity
    public void SetMouseSens(float sens)
    {
        mouseSens = (int)sens;
    }
    //Sets the text for sens in the menu
    public void SetTextSens(Text text)
    {
        text.text = mouseSens.ToString();
    }
    //Sets whether to invert y axis
    public void SetInvertY(bool invert)
    {
        invertY = invert;
    }
}
