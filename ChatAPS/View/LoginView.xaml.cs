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
            conexao con = new conexao();
            if (Conectado == false)
            {
                // Inicializa a conexão
               Conectado = con.InicializaConexao(txtIP.Text,txtUsuario.Text);
                if (Conectado)
                { 
                    ChatView fmr = new ChatView();
                    //chama proxima tela
                    fmr.Show();
                    //fecha essa tela
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Erro ao estabelecer conexao");
                }
            }
            else
            {
                MessageBox.Show("Erro ao estabelecer conexao");
            }
        }
    }
}
