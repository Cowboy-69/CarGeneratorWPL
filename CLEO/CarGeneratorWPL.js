/// <reference path=".config/gta_iv.d.ts" />
/// <reference no-default-lib="true" />

var PlayerChar = new Player(0).getChar();

const SaveKey = 89; // Y
const GeneratorFlags = [1632, 1633, 1760, 1888, 1889];

function SaveCarGeneratorToLogFile() {
    if (!Char.DoesExist(PlayerChar)) {
        Text.PrintStringWithLiteralStringNow("string", "CarGeneratorWPL: ~r~Can't save at the moment", 4000, true);
        return;
    }

    if (!PlayerChar.isInAnyCar()) {
        return;
    }

    let playerCar = PlayerChar.getCarIsUsing();

    let carPosX = playerCar.getCoordinates().x;
    let carPosY = playerCar.getCoordinates().y;
    let carPosZ = playerCar.getCoordinates().z;
    let carHeading = playerCar.getHeading();

    let headingInDegrees = carHeading * (Math.PI / 180.0);
    let forwardX = -Math.sin(headingInDegrees) * 7.0;
    let forwardY = Math.cos(headingInDegrees) * 7.0;

    let currentFlag = GeneratorFlags[Math.RandomIntInRange(0, GeneratorFlags.length)];

    let line1 = `${carPosX}, ${carPosY}, ${carPosZ}, ${forwardX}, ${forwardY}, `;
    let line2 = `3, hash:0, -1, -1, -1, -1, ${currentFlag}, 0, 0\n`;
    
    let file = File.Open("CLEO/CarGeneratorWPL.log");
    if (file == -1) {
        Text.PrintStringWithLiteralStringNow("string", "CarGeneratorWPL: ~r~Failed to save the car generator!", 4000, true);
    } else {
        file.writeString(line1);
        file.writeString(line2);
        file.close();

        Text.PrintStringWithLiteralStringNow("string", "CarGeneratorWPL: The car generator is saved", 3000, true);
    }
}

while(true) {
    wait(0);

    if (Pad.IsKeyDown(SaveKey)) {
        SaveCarGeneratorToLogFile();
    }
}
