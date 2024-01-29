import edu.princeton.cs.algs4.StdOut;

import java.util.Arrays;

public class WeakLearner {
    private int dp; // dimension predictor
    private double vp; // value predictor
    private int sp; // sign predictor
    private int k; // number of locations

    private class InputIndex implements Comparable<InputIndex> {
        private double input; // input value associated to initial index
        private int index; // initial index

        // constructor for InputIndex
        public InputIndex(double input, int index) {
            this.input = input;
            this.index = index;
        }

        // returns input key
        public double getInput() {
            return this.input;
        }

        // returns index value
        public int getIndex() {
            return this.index;
        }

        // comparison function, compare by input
        public int compareTo(InputIndex input1) {
            return Double.compare(this.input, input1.getInput());
        }
    }

    // train the weak learner
    public WeakLearner(double[][] input, double[] weights, int[] labels) {
        if (input == null || weights == null || labels == null)
            throw new IllegalArgumentException("arguments are null");

        for (int i = 0; i < weights.length; i++) {
            if (weights[i] < 0)
                throw new IllegalArgumentException("weight is negative");
        }

        for (int i = 0; i < labels.length; i++) {
            if (labels[i] != 0 && labels[i] != 1)
                throw new IllegalArgumentException("label is not 0 or 1");
            if (input[i] == null)
                throw new IllegalArgumentException("transaction summary is null");
        }

        if (input.length != weights.length || input.length != labels.length)
            throw new IllegalArgumentException("lengths are incompatible");

        this.k = input[0].length;
        int n = input.length;

        double maxWeight = 0;

        for (int i = 0; i < this.k; i++) {
            double weight0 = 0.0;
            double weight1 = 0.0;

            InputIndex[] st = new InputIndex[n];

            for (int j = 0; j < n; j++) {
                st[j] = new InputIndex(input[j][i], j);
                if (labels[j] == 1) weight0 += weights[j];
                if (labels[j] == 0) weight1 += weights[j];
            }

            Arrays.sort(st);

            for (int j = 0; j < n; j++) {
                int index = st[j].getIndex();
                double val = st[j].getInput();

                if (labels[index] == 1) {
                    weight0 -= weights[index];
                    weight1 += weights[index];
                }
                else {
                    weight0 += weights[index];
                    weight1 -= weights[index];
                }

                if (j < n - 1 && val == st[j + 1].getInput())
                    continue;

                if (weight0 > maxWeight) {
                    this.dp = i;
                    this.vp = val;
                    this.sp = 0;
                    maxWeight = weight0;
                }

                if (weight1 > maxWeight) {
                    this.dp = i;
                    this.vp = val;
                    this.sp = 1;
                    maxWeight = weight1;
                }
            }
        }
    }

    // return the prediction of the learner for a new sample
    public int predict(double[] sample) {
        if (sample == null)
            throw new IllegalArgumentException("arguments are null");

        if (sample.length != k)
            throw new IllegalArgumentException("lengths are incompatible");

        if (sp == 0) {
            if (sample[dimensionPredictor()] <= vp) return 0;
            else return 1;
        }
        else {
            if (sample[dimensionPredictor()] <= vp) return 1;
            else return 0;
        }
    }

    // return the dimension the learner uses to separate the data
    public int dimensionPredictor() {
        return dp;
    }

    // return the value the learner uses to separate the data
    public double valuePredictor() {
        return vp;
    }

    // return the sign the learner uses to separate the data
    public int signPredictor() {
        return sp;
    }

    // unit testing (required)
    public static void main(String[] args) {
        DataSet training = new DataSet(args[0]);
        DataSet test = new DataSet(args[1]);

        double[] weights = new double[training.n];

        for (int i = 0; i < weights.length; i++) {
            weights[i] = 1.0;
        }

        // train the model
        WeakLearner model = new WeakLearner(training.input, weights, training.labels);

        // calculate the training data set accuracy
        double trainingAccuracy = 0;
        for (int i = 0; i < training.n; i++)
            if (model.predict(training.input[i]) == training.labels[i])
                trainingAccuracy += 1;
        trainingAccuracy /= training.n;

        // calculate the test data set accuracy
        double testAccuracy = 0;
        for (int i = 0; i < test.n; i++)
            if (model.predict(test.input[i]) == test.labels[i])
                testAccuracy += 1;
        testAccuracy /= test.n;

        StdOut.println("Training accuracy of model: " + trainingAccuracy);
        StdOut.println("Test accuracy of model:     " + testAccuracy);
        StdOut.println("Value Predictor: " + model.valuePredictor());
        StdOut.println("Dimension Predictor: " + model.dimensionPredictor());
        StdOut.println("Sign Predictor: " + model.signPredictor());
    }
}
