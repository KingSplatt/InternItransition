import java.util.List;

public class MainGame {
    public static void main(String[] args) {
        try {
            DiceArg diceArg = new DiceArg();
            List<Dice> diceList = diceArg.recieveDice(args);
            GameEngine gameEngine = new GameEngine(diceList);
            gameEngine.startGame();
        } catch (IllegalArgumentException e) {
            System.err.println("Error: " + e.getMessage());
        } catch (Exception e) {
            System.err.println("Error: " + e.getMessage());
        }
    }
}