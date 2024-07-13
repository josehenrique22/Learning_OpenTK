
namespace BasicOpenTK{

    class Program
    {
        // recebe a instancia de Game e executa os parametros definidos pelo Costrutor
        static void Main(string[] args)
        {
            using(Game game = new Game(800, 600, "LearnOpenTK"))
            {
                game.Run();
            }
        }
    }
}