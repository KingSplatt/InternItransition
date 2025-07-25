import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;

public class GameEngine {
    private List<Dice> diceList;
    private Scanner sc;
    private FairRandomGenerator fairRanGen;
    private Dice computerDice;
    private Dice userDice;
    private boolean computerStarts;

    public GameEngine(List<Dice> diceList) {
        this.diceList = new ArrayList<>(diceList);
        this.sc = new Scanner(System.in);
        this.fairRanGen = new FairRandomGenerator();
    }

    public void startGame() {
        showDices();
        System.out.println("Let's determine who makes the first move.");
        determineFirstMove();
        selectDice();
        playRounds();
        showProbabilityTable();
    }

    public void showDices() {
        System.out.println("Available dice's:");
        for (int i = 0; i < diceList.size(); i++) {
            System.out.println(i + " - " + diceToString(diceList.get(i)));
        }
        System.out.println();
    }

    public void determineFirstMove() {
        String hmac = fairRanGen.generateFairRandom(2);
        System.out.println("I selected a random value in the range 0..1 (HMAC=" + hmac + ").");
        System.out.println("Try to guess my selection.");
        showOptions(2);
        int userChoice = getUserChoice(2);
        int computerNumber = fairRanGen.getComputerNumber();
        String key = fairRanGen.getKey();
        System.out.println("My selection: " + computerNumber + " (KEY=" + key + ").");
        computerStarts = (computerNumber != userChoice);
    }

    public void selectDice() {
        if (computerStarts) {
            computerDice = diceList.get(0);
            System.out.println("I make the first move and choose the " + diceToString(computerDice) + " dice.");
            System.out.println("Choose your dice:");
            List<Dice> availableDice = new ArrayList<Dice>();
            for (int i = 1; i < diceList.size(); i++) {
                availableDice.add(diceList.get(i));
                System.out.println(i-1 + " - " + diceToString(diceList.get(i)));
            }
            showExitAndHelp();
            int userDiceIndex = getUserChoice(availableDice.size()) + 1;
            userDice = diceList.get(userDiceIndex);
            System.out.println("You choose the " + diceToString(userDice) + " dice.");
        } else {
            System.out.println("You make the first move. Choose your dice:");
            for (int i = 0; i < diceList.size(); i++) {
                System.out.println(i + " - " + diceToString(diceList.get(i)));
            }
            showExitAndHelp();
            int userDiceIndex = getUserChoice(diceList.size());
            userDice = diceList.get(userDiceIndex);
            System.out.println("You choose the " + diceToString(userDice) + " dice.");
            List<Dice> availableDice = new ArrayList<>(diceList);
            availableDice.remove(userDiceIndex);
            computerDice = availableDice.get(0);
            System.out.println("I choose the " + diceToString(computerDice) + " dice.");
        }
    }

    public void playRounds() {
        if (computerStarts) {
            int computerRoll = rollDice(computerDice, "It's time for my roll.");
            int userRoll = rollDice(userDice, "It's time for your roll.");
            determineWinner(computerRoll, userRoll, false);
        } else {
            int userRoll = rollDice(userDice, "It's time for your roll.");
            int computerRoll = rollDice(computerDice, "It's time for my roll.");
            determineWinner(computerRoll, userRoll, true);
        }
    }

    public int rollDice(Dice dice, String message) {
        System.out.println(message);
        int diceSize = dice.getFaces().length;
        String hmac = fairRanGen.generateFairRandom(diceSize);
        System.out.println("I selected a random value in the range 0.." + (diceSize-1) + " (HMAC=" + hmac + ").");
        System.out.println("Add your number modulo " + diceSize + ".");
        showOptions(diceSize);
        int userNumber = getUserChoice(diceSize);
        int computerNumber = fairRanGen.getComputerNumber();
        String key = fairRanGen.getKey();
        System.out.println("My number is " + computerNumber + " (KEY=" + key + ").");
        int fairResult = fairRanGen.getFairResult(userNumber, diceSize);
        System.out.println("The fair number generation result is " + computerNumber + " + " + userNumber + " = " + fairResult + " (mod " + diceSize + ").");
        int rollResult = dice.getFaceValue(fairResult);
        System.out.println((dice == computerDice ? "My" : "Your") + " roll result is " + rollResult + ".");
        return rollResult;
    }

    public int getUserChoice(int maxOptions) {
        while (true) {
            String input = sc.nextLine().trim();
            if (input.equalsIgnoreCase("X")) {
                System.out.println("Game ended by user.");
                System.exit(0);
            } else if (input.equals("?")) {
                System.out.println(ProbCalculate.genProbabilityTable(diceList));
                showOptions(maxOptions);
                continue;
            }
            try {
                int choice = Integer.parseInt(input);
                if (choice >= 0 && choice < maxOptions) 
                    return choice;
                System.out.println("Please enter a number between 0 and " + (maxOptions-1) + ".");
            } catch (NumberFormatException e) {
                System.out.println("Please enter a valid number or X to exit.");
            }
        }
    }

    public void determineWinner(int computerRoll, int userRoll, boolean userFirst) {
        if (userRoll > computerRoll) {
            System.out.println("You win (" + userRoll + " > " + computerRoll + ")!");
        } else if (computerRoll > userRoll) {
            System.out.println("I win (" + computerRoll + " > " + userRoll + ")!");
        } else
            System.out.println("It's a tie (" + computerRoll + " = " + userRoll + ")!");
    }

    public String diceToString(Dice dice) {
        StringBuilder sb = new StringBuilder("[");
        int faces[] = dice.getFaces();
        for (int i = 0; i < faces.length; i++) {
            sb.append(faces[i]);
            if (i < faces.length - 1) 
                sb.append(",");
        }
        sb.append("]");
        return sb.toString();
    }

    public void showProbabilityTable() {
        System.out.println("\n" + ProbCalculate.genProbabilityTable(diceList));
    }
    
    public void showOptions(int maxOptions) {
        for (int i = 0; i < maxOptions; i++) {
            System.out.println(i + " - " + i);
        }
        showExitAndHelp();
    }
    
    public void showExitAndHelp() {
        System.out.println("X - exit");
        System.out.println("? - help");
    }
}
