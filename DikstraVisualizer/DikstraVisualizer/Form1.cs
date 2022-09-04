namespace DikstraVisualizer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {  
            #region dijstra things
            /*
            Graph<string> graphOfLetters = new Graph<string>();
            Vertex<string> Find(string letter)
            {
                return graphOfLetters.Search(letter);
            }
            graphOfLetters.AddVertex("a");
            graphOfLetters.AddVertex("b");
            graphOfLetters.AddVertex("c");
            graphOfLetters.AddVertex("d");
            graphOfLetters.AddVertex("e");
            graphOfLetters.AddVertex("f");
            graphOfLetters.AddEdge(Find("e"), Find("b"),7);
            graphOfLetters.AddEdge(Find("d"), Find("c"),8);
            graphOfLetters.AddEdge(Find("b"), Find("d"),3);
            graphOfLetters.AddEdge(Find("c"), Find("a"),19);
            graphOfLetters.AddEdge(Find("a"), Find("e"),1);
            graphOfLetters.AddEdge(Find("c"), Find("d"),15);
            graphOfLetters.AddEdge(Find("e"), Find("f"),10);
            graphOfLetters.AddEdge(Find("f"), Find("b"),13);
            graphOfLetters.AddEdge(Find("b"), Find("a"),17);

            List<Vertex<string>> list = graphOfLetters.DijkstrasAlgorithm(Find("a"), Find("c"));

            foreach(var vertex in list)
            {
                Console.WriteLine(vertex.Value);
            }
            */
            #endregion
        }
    }
}