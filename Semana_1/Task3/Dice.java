public class Dice {
    private int[] faces;

    public Dice(int[] faces) {
        this.faces = faces;
    }

    public int[] getFaces() {
        return faces;
    }

    public int getFaceValue(int index) {
        if (index < 0 || index >= faces.length) 
            throw new IndexOutOfBoundsException("Index out of bounds for faces");
        return faces[index];
    }

    public void setFaces(int[] faces) {
        this.faces = faces;
    }


}
