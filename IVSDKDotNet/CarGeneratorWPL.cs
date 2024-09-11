using System;
using System.Numerics;
using System.Windows.Forms;
using System.IO;

using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;

namespace CarGeneratorWPL
{
    public class CarGeneratorWPL : Script
    {
        readonly int[] GeneratorFlags = { 1632, 1633, 1760, 1888, 1889, };

        Random RandomFlag = new Random();

        public CarGeneratorWPL()
        {
            KeyDown += OnKeyDown;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Y)
            {
                return;
            }

            SaveCarGeneratorToLogFile();
        }

        private void SaveCarGeneratorToLogFile()
        {
            int PlayerIndex = CONVERT_INT_TO_PLAYERINDEX(GET_PLAYER_ID());

            int PlayerChar;
            GET_PLAYER_CHAR(PlayerIndex, out PlayerChar);

            if (!DOES_CHAR_EXIST(PlayerChar))
            {
                PRINT_STRING_WITH_LITERAL_STRING_NOW("string", "CarGeneratorWPL: ~r~Can't save at the moment", 4000, true);
                return;
            }

            if (!IS_CHAR_IN_ANY_CAR(PlayerChar))
            {
                return;
            }

            int PlayerVehicle;
            GET_CAR_CHAR_IS_USING(PlayerChar, out PlayerVehicle);

            Vector3 VehiclePosition;
            GET_CAR_COORDINATES(PlayerVehicle, out VehiclePosition);

            float VehicleHeading;
            GET_CAR_HEADING(PlayerVehicle, out VehicleHeading);

            double HeadingInDegrees = VehicleHeading * (Math.PI / 180.0);
            double ForwardX = -Math.Sin(HeadingInDegrees) * 7.0;
            double ForwardY = Math.Cos(HeadingInDegrees) * 7.0;

            int CurrentFlag = RandomFlag.Next(0, GeneratorFlags.Length);

            string[] Line = { string.Format("{0}, {1}, {2}, {3:f3}, {4:f3}, 3, hash:0, -1, -1, -1, -1, {5}, 0, 0",
                                            VehiclePosition.X, VehiclePosition.Y, VehiclePosition.Z, ForwardX, ForwardY, GeneratorFlags[CurrentFlag]) };

            File.AppendAllLines(Path.Combine("IVSDKDotNet\\scripts\\", "CarGeneratorWPL.ivsdk.log"), Line);

            PRINT_STRING_WITH_LITERAL_STRING_NOW("string", "CarGeneratorWPL: The car generator is saved", 3000, true);
        }
    }
}
