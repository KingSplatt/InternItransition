import java.util.ArrayList;
import java.util.List;

public class DiceArg {
    public List<Dice> recieveDice(String[] args) {
        if(args.length < 3 ) throw new IllegalArgumentException("You need to give at least 3 dice arguments");
        List<Dice> diceL = new ArrayList<Dice>();
        for (String arg : args) {
            String faceValues[] = arg.split(",");
            int faces[] = new int[faceValues.length];
            try {
                for (int i = 0; i < faceValues.length; i++) {
                    faces[i] = Integer.parseInt(faceValues[i].trim());
                    if(faces[i] > 9 || faces[i] < 0) {
                        throw new IllegalArgumentException("Dice faces must be between 0 and 9. Invalid dice: " + arg);
                    }
                    faces[i] = Integer.parseInt(faceValues[i].trim());
                }
            } catch (NumberFormatException e) {
                throw new IllegalArgumentException("Invalid face value in argument: " + arg, e);
            }
            validateDiceFaces(faces, arg);
            diceL.add(new Dice(faces));
        }
        return diceL;
    }
    
    private void validateDiceFaces(int[] faces, String originalArg) {
        if (faces.length < 2) {
            throw new IllegalArgumentException("Each dice must have at least 2 faces. Invalid dice: " + originalArg);
        }
        for (int face : faces) {
            if (face < 0) {
                throw new IllegalArgumentException("Dice faces cannot have negative values. Invalid dice: " + originalArg);
            }
        }
    }
}
