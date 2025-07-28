import java.security.SecureRandom;
import javax.crypto.Mac;
import javax.crypto.spec.SecretKeySpec;

public class FairRandomGenerator {
    private String HMAC;
    private int computerNumber;
    private byte[] key;
    private final SecureRandom sRandom = new SecureRandom();
    private static final int MAGIC_VALUE = 32;


    public String generateFairRandom(int range){
        key = new byte[MAGIC_VALUE];
        sRandom.nextBytes(key);
        computerNumber = sRandom.nextInt(range);
        try {
            Mac mac = Mac.getInstance("HmacSHA256");
            SecretKeySpec SPEC = new SecretKeySpec(key, "HmacSHA256");
            mac.init(SPEC);
            byte[] hmacByt = mac.doFinal(String.valueOf(computerNumber).getBytes());
            HMAC = java.util.HexFormat.of().formatHex(hmacByt).toUpperCase();
        } catch (Exception e) {
            throw new RuntimeException("Error generating HMAC", e);
        }
        return HMAC;
    }

    public int getFairResult(int userNumber, int range){
        return (computerNumber + userNumber) % range;
    }

    public int getComputerNumber() {
        return computerNumber;
    }

    public String getKey() {
        return java.util.HexFormat.of().formatHex(key).toUpperCase();
    }

    public String getHMAC() {
        return HMAC;
    }

    public void setHMAC(String hmac) {
        this.HMAC = hmac;
    }
    
    public boolean verifyHMAC(String providedKey, int providedNumber, String providedHMAC) {
        try {
            byte[] keyBytes = new byte[providedKey.length() / 2];
            for (int i = 0; i < providedKey.length(); i += 2) {
                keyBytes[i / 2] = (byte) Integer.parseInt(providedKey.substring(i, i + 2), 16);
            }
            Mac mac = Mac.getInstance("HmacSHA256");
            SecretKeySpec spec = new SecretKeySpec(keyBytes, "HmacSHA256");
            mac.init(spec);
            byte[] hmacBytes = mac.doFinal(String.valueOf(providedNumber).getBytes());
            StringBuilder sb = new StringBuilder();
            for (byte b : hmacBytes) {
                sb.append(String.format("%02X", b));
            }
            String calculatedHMAC = sb.toString();
            return calculatedHMAC.equals(providedHMAC);
        } catch (Exception e) {
            return false;
        }
    }
}
