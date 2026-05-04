using System;
using System.Windows.Forms;

namespace Tamagotchi
{
    public partial class VPScreen : Form
    {
        private Timer TamagotchiHeartbeat;

        public VPScreen()
        {
            InitializeComponent();
        }

        private void TamagotchiHeartbeat_Tick(object sender, EventArgs e)
        {
            // Handle hunger/happiness over time
            UpdatePetLogic();

            // Draw the new frame
            RenderGame();
        }

        /// <summary>
        /// Update the pet's logic, such as hunger and happiness levels, based on time and interactions.
        /// </summary>
        private void UpdatePetLogic()
        {
           
        }

        /// <summary>
        /// Simple render engine gives you total control over the "retro" look.
        /// </summary>
        private void RenderGame()
        {
            
        }

        private void VPScreen_Shown(object sender, EventArgs e)
        {
            // Create the timer that will drive the game loop
            TamagotchiHeartbeat = new Timer
            {
                // Set this timer to 100ms or 200ms for a "retro" frame rate
                Interval = 100,
                Enabled = true
            };

            // Start the game loop
            TamagotchiHeartbeat.Tick += TamagotchiHeartbeat_Tick;

            
        }
    }
}
