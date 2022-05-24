using ChatAPS.Controler;
using System.Windows;
using System.Windows.Input;

namespace ChatAPS.View
{
    /// <summary>
    /// Interaction logic for ChatView.xaml
    /// </summary>
    public partial class ChatView : Window
    {
        LoginView frm = new LoginView();
        conexao con = new conexao();
        public ChatView()
        {
            InitializeComponent();
            while (frm.Conectado)
            {
                con.AtualizaLog(txtChat.Text);
            }
        }

        private void btnFechar_Click(object sender, RoutedEventArgs e)
        {      
            con.FechaConexao("Desconectado a pedido do usuário.");

            if (frm.Conectado)
            {
                frm.ShowDialog();
                Close();
            }
            else
            {
                MessageBox.Show("Erro para desconectar!");
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }       
    }
}
