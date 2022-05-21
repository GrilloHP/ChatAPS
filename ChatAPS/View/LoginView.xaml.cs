using ChatAPS.Controler;
using System.Windows;
using System.Windows.Input;

namespace ChatAPS.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public bool Conectado = false;
        conexao con = new conexao();

        public LoginView()
        {
            InitializeComponent();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();            
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnConectar_Click(object sender, RoutedEventArgs e)
        {
            if (Conectado == false)
            {
                // Inicializa a conexão
               Conectado = con.InicializaConexao(txtIP.Text,txtUsuario.Text);
                if (Conectado)
                {
                    //chama proxima tela
                    this.Close();
                }
            }
            else // Se esta conectado entao desconecta
            {
                Conectado = con.FechaConexao("Desconectado a pedido do usuário.");
            }
        }
    }
}
