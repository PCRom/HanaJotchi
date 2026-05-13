using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HanaJotchi
{
    [DesignerCategory("Form")]
    public partial class VPScreen : GotchaGotchi
    {
        public Timer HanaJotchiHeartbeat { get; private set; }

        /// <summary>
        /// Initializes a new instance of the VPScreen class, optionally enabling private server mode.
        /// </summary>
        /// <remarks>This constructor sets the window to remain on top of other windows. If private server
        /// mode is enabled, the UsePrivateServer property is set accordingly.</remarks>
        /// <param name="enablePrivateServer">true to enable private server mode; otherwise, false. The default is false.</param>
        public VPScreen(bool enablePrivateServer = false)
        {
            InitializeComponent();
            
            // Keep the window on top
            this.TopMost = true;

            // Check for private server configuration
            if (enablePrivateServer)
            {
                // Load lines from the config file, we expect the private server URL to be on line 6 (index 5)
                string[] array = File.ReadAllLines("HanaJotchiPrivate.cfg");

                // We loop through each line in the configuration file to find the server URL. This allows for flexible configuration and the ability to add additional settings in the future without changing the code, as we can simply look for specific keys in the config file.
                foreach (string line in array)
                {
                    // If the line starts with "//", we treat it as a comment and ignore it, otherwise we use the provided URL as the API endpoint.
                    if (!line.StartsWith("//") && line.Contains("="))
                    {
                        // We split the line on the '=' character and take the second part as the API endpoint URL, allowing for flexible configuration without hardcoding the server address.
                        switch (line.Split('=')[0])
                        {
                            case "server": // If the line starts with "server=", we take the value after the '=' as the API endpoint URL and set it to the apiEndpoint variable, allowing the application to connect to a private server instead of the default official server.
                                apiEndpoint = line.Split('=')[1];
                                break;
                            default:
                                MessageBox.Show($"Found missing config line: \n{line}");
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the timer tick event to update the virtual pet's state and refresh the game display.
        /// </summary>
        /// <remarks>This method is intended to be used as an event handler for a timer that periodically
        /// updates the virtual pet's logic and redraws the game interface. It should be attached to the timer's Tick
        /// event to ensure regular updates.</remarks>
        /// <param name="sender">The source of the event, typically the timer that triggered the tick.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        private void HanaJotchiHeartbeat_Tick(object sender, EventArgs e)
        {
            // Handle hunger/happiness over time
            UpdatePetLogic(CanvasBox);

            // Draw the new frame
            RenderGame(CanvasBox);
        }


        /// <summary>
        /// Handles the Shown event of the VPScreen form, initializing UI elements, loading configuration settings, and
        /// starting the main game loop timer.
        /// </summary>
        /// <remarks>This method sets up the initial state of the VPScreen, including positioning UI
        /// controls, reading private server configuration if enabled, and initializing the pet and game loop timer. It
        /// is intended to be called automatically when the form is first displayed.</remarks>
        /// <param name="sender">The source of the event, typically the VPScreen form instance.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        private void VPScreen_Shown(object sender, EventArgs e)
        {
            // Position the exit button in the top-right corner of the PictureBox
            exitBtn = new Rectangle(CanvasBox.Width - 40, 0, 40, 40);


            // Initialize pet data (in a real game, this would come from the API)
            hanaJotchiPet = new GotchaGotchiPet
            {
                Name = "Fluffy",
                Token = "abc123", //This Token would be a unique identifier from the API, it is the lifeline to your pet's data and must be included in all API calls to update or retrieve stats.
                Age = 2,
                Description = "A happy little pet.",
                Experience = 79,
                Happiness = 67,
                Hunger = 60,
                IsAwake = true,
                Species = "Cat",
            };

            // Create the timer that will drive the game loop
            HanaJotchiHeartbeat = new Timer
            {
                // Set this timer to 100ms or 200ms for a "retro" frame rate
                Interval = 100,
                Enabled = true
            };

            // Start the game loop
            HanaJotchiHeartbeat.Tick += HanaJotchiHeartbeat_Tick;

        }

        /// <summary>
        /// Handles the MouseClick event for the PictureBox, triggering actions when interactive buttons within the
        /// PictureBox are clicked, determining if any interactive buttons were clicked and performing the corresponding actions.
        /// </summary>
        /// <remarks>This method determines which interactive button, if any, was clicked within the
        /// PictureBox and performs the corresponding action. Ensure that the button regions are properly defined to
        /// enable correct interaction.</remarks>
        /// <param name="sender">The source of the event, typically the PictureBox control.</param>
        /// <param name="e">A MouseEventArgs that contains the event data, including the location of the mouse click.</param>
        private void CanvasBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (feedBtn.Contains(e.Location))
            {
                PerformAction("feed");
            }
            else if (playBtn.Contains(e.Location))
            {
                PerformAction("play");
            }
            else if (sleepBtn.Contains(e.Location))
            {
                PerformAction("sleep");
            }
            else if (exitBtn.Contains(e.Location))
            {
                PerformAction("exit");
            }
        }

        /// <summary>
        /// Performs the specified action on the virtual pet, such as feeding, playing, or putting it to sleep.
        /// </summary>
        /// <remarks>If the action is "exit", the application will close. For other supported actions, the
        /// pet's state is updated and the corresponding behavior is triggered. After performing the action, the pet's
        /// state resets to idle after a short delay.</remarks>
        /// <param name="action">The action to perform. Supported values are "feed", "play", "sleep", and "exit". The value is
        /// case-sensitive.</param>
        private async void PerformAction(string action)
        {
            petState = "[Debug State] " + action;
            RenderGame(CanvasBox);

            // Update stats via API

            switch(action)
            {
                case "feed":
                    hanaJotchiPet.Feed();
                    break;
                case "play":
                    hanaJotchiPet.Play();
                    break;
                case "sleep":
                    hanaJotchiPet.Sleep();
                    break;
                case "exit":
                    Application.Exit();
                    break;
            }

            // Reset state after a moment
            await Task.Delay(1000);
            petState = "Idle";
        }
  
        /// <summary>
        /// Handles the MouseDown event for the PictureBox to initiate a drag operation when the left mouse button is
        /// pressed.
        /// </summary>
        /// <remarks>Dragging is initiated only when the left mouse button is pressed. This event is
        /// commonly used to enable moving or interacting with the PictureBox via mouse input.</remarks>
        /// <param name="sender">The source of the event, typically the PictureBox control.</param>
        /// <param name="e">A MouseEventArgs that contains the event data, including information about which mouse button was pressed.</param>
        private void CanvasBox_MouseDown(object sender, MouseEventArgs e)
        {
            // Drag as we are pressing on any image render of the object,
            // the side effect of a picturebox will drag everything except transparent
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastCursorPos = Cursor.Position;
                lastFormPos = Location;
            }
        }

        /// <summary>
        /// Handles the MouseMove event for the PictureBox control to update the form's position while dragging.
        /// </summary>
        /// <remarks>This method enables the user to move the form by clicking and dragging the PictureBox
        /// control when dragging is active. Dragging behavior depends on the state of the isDragging flag and the
        /// tracking of previous cursor and form positions.</remarks>
        /// <param name="sender">The source of the event, typically the PictureBox control.</param>
        /// <param name="e">A MouseEventArgs that contains the event data, including the current mouse position.</param>
        private void CanvasBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                // Calculate how much the mouse has moved since the last frame
                int diffX = Cursor.Position.X - lastCursorPos.X;
                int diffY = Cursor.Position.Y - lastCursorPos.Y;

                // Update the form's position
                Location = new Point(lastFormPos.X + diffX, lastFormPos.Y + diffY);
            }
        }

        /// <summary>
        /// Handles the MouseUp event for the picture box to end a drag operation.
        /// </summary>
        /// <param name="sender">The source of the event, typically the picture box control.</param>
        /// <param name="e">A MouseEventArgs that contains the event data for the mouse button release.</param>
        private void CanvasBox_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }
    }
}