import edu.princeton.cs.algs4.CC;
import edu.princeton.cs.algs4.Edge;
import edu.princeton.cs.algs4.EdgeWeightedGraph;
import edu.princeton.cs.algs4.In;
import edu.princeton.cs.algs4.KruskalMST;
import edu.princeton.cs.algs4.Point2D;
import edu.princeton.cs.algs4.StdOut;

public class Clustering {
    private int m; // number of locations
    private CC clusters; // clustered locations

    // run the clustering algorithm and create the clusters
    public Clustering(Point2D[] locations, int k) {
        if (locations == null)
            throw new IllegalArgumentException("argument is null");

        for (int i = 0; i < locations.length; i++) {
            if (locations[i] == null)
                throw new IllegalArgumentException("element is null");
        }

        m = locations.length;

        if (k > m || k < 1)
            throw new IllegalArgumentException("k out of range");

        EdgeWeightedGraph graph = new EdgeWeightedGraph(m);

        for (int i = 0; i < m; i++) {
            for (int j = i + 1; j < m; j++) {
                graph.addEdge(new Edge(i, j, locations[i].distanceTo(locations[j])));
            }
        }

        KruskalMST mst = new KruskalMST(graph);
        EdgeWeightedGraph clusterGraph = new EdgeWeightedGraph(m);

        int index = 0;
        for (Edge edge : mst.edges()) {
            if (index < m - k)
                clusterGraph.addEdge(edge);
            index++;
        }

        clusters = new CC(clusterGraph);
    }

    // return the cluster of the ith point
    public int clusterOf(int i) {
        if (i < 0 || i >= m)
            throw new IllegalArgumentException("i out of range");
        return clusters.id(i);
    }

    // use the clusters to reduce the dimensions of an input
    public double[] reduceDimensions(double[] input) {
        if (input == null)
            throw new IllegalArgumentException("input is null");

        if (input.length != m)
            throw new IllegalArgumentException("input wrong length");

        double[] clusterInput = new double[clusters.count()];

        for (int i = 0; i < m; i++) {
            clusterInput[clusters.id(i)] += input[i];
        }

        return clusterInput;
    }

    // unit testing (required)
    public static void main(String[] args) {
        In in = new In("princeton_locations.txt");

        Point2D[] locations = new Point2D[in.readInt() + 1];
        int k = 5;
        int i = -1;
        double[] input = new double[locations.length];

        while (in.hasNextLine()) {
            String line = in.readLine();

            double[] coor = new double[2];
            for (int j = 0; j < line.length(); j++) {
                if (line.charAt(j) == ' ') {
                    coor[0] = Double.parseDouble(line.substring(0, j));
                    coor[1] = Double.parseDouble(line.substring(j + 1));
                    break;
                }
                input[j] = j;
            }
            i++;
            locations[i] = new Point2D(coor[0], coor[1]);
        }

        Clustering clusters = new Clustering(locations, k);
        clusters.reduceDimensions(input);

        for (int j = 0; j < clusters.m; j++) {
            StdOut.println("Cluster of point " + j + ": " + clusters.clusterOf(j));
        }

        StdOut.println("Cluster of point 0: " + clusters.clusterOf(0));
        StdOut.println("Cluster of point 1: " + clusters.clusterOf(1));
        StdOut.println("Cluster of point 2: " + clusters.clusterOf(2));
    }
}

