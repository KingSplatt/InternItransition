import java.util.Arrays;
import java.util.List;

public class ProbCalculate {
    public static double calculateWinProb(Dice d1, Dice d2){
        double wins = 0.0;
        double total = d1.getFaces().length * d2.getFaces().length;
        for (int i = 0; i < d1.getFaces().length; i++) {
            for (int j = 0; j < d2.getFaces().length; j++) {
                if (d1.getFaceValue(i) > d2.getFaceValue(j)) {
                    wins++;
                }
            }
        }
        return wins/total;
    }

    public static String genProbabilityTable(List<Dice> diceL){
        StringBuilder table = new StringBuilder();
        table.append(getHeader(diceL));
        table.append(String.format("%20s ", ""));
        table.append("\n");
        table.append(getRows(diceL));
        return table.toString();
    }

    public static String getHeader(List<Dice> diceL) {
        StringBuilder header = new StringBuilder();
        header.append("Probability of the win for the user:\n");
        header.append(String.format("%20s", ""));
        for (Dice dice : diceL) 
            header.append(String.format("%20s", Arrays.toString(dice.getFaces())));
        header.append("\n");
        return header.toString();
    }

    public static String getRows(List<Dice> diceList) {
        StringBuilder rows = new StringBuilder();
        for (Dice dice : diceList) {
            rows.append(String.format("%18s", Arrays.toString(dice.getFaces())));
            for (Dice otherDice : diceList) {
                double prob = calculateWinProb(dice, otherDice);
                rows.append(String.format("%18.1f%%", prob * 100));
            }
            rows.append("\n");
        }
        return rows.toString();
    }
}