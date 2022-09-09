using GameApp;

var game = new Game();

if (!game.Initialize())
{
    return -1;
}

game.RunLoop();
game.Shutdown();
return  0;
