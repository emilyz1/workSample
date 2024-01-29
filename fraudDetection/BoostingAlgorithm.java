import edu.princeton.cs.algs4.Point2D;
import edu.princeton.cs.algs4.StdOut;
import edu.princeton.cs.algs4.Stopwatch;

import java.util.ArrayList;

public class BoostingAlgorithm {
    private double[][] input; // cluster input
    private ArrayList<WeakLearner> weakLearners; // weak learners per iteration
    private double[] weights; // Boosting Algorithm weights
    private int[] labels; // given clean/fraud labels
    private int locations; // number of locations
    private Clustering clusters; // clustered locations
    private int iterations; // number of iterations

    // create the clusters and initialize your data structures
    public BoostingAlgorithm(double[][] input, int[] labels, Point2D[] locations,
                             int k) {
        if (input == null || labels == null || locations == null)
            throw new IllegalArgumentException("argument is null");

        for (int i = 0; i < locations.length; i++) {
            if (locations[i] == null)
                throw new IllegalArgumentException("locations element is null");
        }
        for (int i = 0; i < labels.length; i++) {
            if (input[i] == null)
                throw new IllegalArgumentException("transaction summary is null");
        }

        if (input.length != labels.length || input[0].length != locations.length)
            throw new IllegalArgumentException("lengths are incompatible");

        clusters = new Clustering(locations, k);
        this.labels = new int[input.length];
        this.input = new double[input.length][k];
        this.locations = input[0].length;
        this.weights = new double[input.length];
        this.weakLearners = new ArrayList<WeakLearner>();
        this.iterations = 0;

        for (int i = 0; i < input.length; i++) {
            this.labels[i] = labels[i];
            this.input[i] = clusters.reduceDimensions(input[i]);
            this.weights[i] = 1.0 / (double) input.length;
        }
    }

    // return the current weights
    public double[] weights() {
        double[] weight = this.weights;
        return weight;
    }

    // apply one step of the boosting algorithm
    public void iterate() {
        WeakLearner wl = new WeakLearner(input, weights, labels);

        for (int i = 0; i < input.length; i++) {
            if (wl.predict(input[i]) != labels[i])
                weights[i] *= 2.0;
        }

        double sum = 0.0;
        for (int i = 0; i < weights.length; i++) {
            sum += weights[i];
        }

        for (int i = 0; i < weights.length; i++) {
            weights[i] = weights[i] / sum;
        }
        weakLearners.add(wl);
        iterations++;
    }

    // return the prediction of the learner for a new sample
    public int predict(double[] sample) {
        if (sample == null || sample.length != locations)
            throw new IllegalArgumentException("sample is incompatible");

        int clean = 0;
        int fraud = 0;

        sample = clusters.reduceDimensions(sample);

        for (int i = 0; i < iterations; i++) {
            if (weakLearners.get(i).predict(sample) == 0) clean++;
            else fraud++;
        }

        if (clean >= fraud) return 0;
        else return 1;
    }

    // unit testing (required)
    public static void main(String[] args) {
        // read in the terms from a file
        DataSet training = new DataSet(args[0]);
        DataSet test = new DataSet(args[1]);
        int k = Integer.parseInt(args[2]);
        int iterations = Integer.parseInt(args[3]);
        Stopwatch watch = new Stopwatch();

        // train the model
        BoostingAlgorithm model = new BoostingAlgorithm(training.input,
                                                        training.labels,
                                                        training.locations, k);
        for (int t = 0; t < iterations; t++)
            model.iterate();

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

        StdOut.println("Time Elapsed: " + watch.elapsedTime());
        StdOut.println("Training accuracy of model: " + trainingAccuracy);
        StdOut.println("Test accuracy of model:     " + testAccuracy);

        double[] weights = model.weights();

        for (int i = 0; i < weights.length; i++) {
            StdOut.println("Weight: " + weights[i]);
        }
    }
}
