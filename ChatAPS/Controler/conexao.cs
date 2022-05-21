using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;

namespace ChatAPS.Controler
{
    public class conexao
    {
        // Trata o nome do usuário
        public StreamWriter stwEnviador;
        public StreamReader strReceptor;
        public TcpClient tcpServidor;
        // Necessário para atualizar o formulário com mensagens da outra thread
        public delegate void AtualizaLogCallBack(string strMensagem);
        // Necessário para definir o formulário para o estado "disconnected" de outra thread
        public delegate void FechaConexaoCallBack(string strMotivo);
        public Thread mensagemThread;
        public IPAddress enderecoIP;
        public bool Conectado;

        public bool InicializaConexao(string servidorIP, string usuario = "Desconhecido")
        {
            try
            {
                // Trata o endereço IP informado em um objeto IPAdress
                enderecoIP = IPAddress.Parse(servidorIP);
                // Inicia uma nova conexão TCP com o servidor chat
                tcpServidor = new TcpClient();
                tcpServidor.Connect(enderecoIP, 2502);

                // Ajuda a verificar se estamos conectados ou não
                Conectado = true;

                // Envia o nome do usuário ao servidor
                stwEnviador = new StreamWriter(tcpServidor.GetStream());
                stwEnviador.WriteLine(usuario);
                stwEnviador.Flush();

                //Inicia a thread para receber mensagens e nova comunicação
                mensagemThread = new Thread(new ThreadStart(RecebeMensagens));
                mensagemThread.Start();
                return true;
            }
            catch (Exception ex)
            {
                return false;

                //colocar uma mensagem na tela falando que deu falha de conexao (front)
            }
        }
        public void RecebeMensagens()
        {
            // recebe a resposta do servidor
            strReceptor = new StreamReader(tcpServidor.GetStream());
            string ConResposta = strReceptor.ReadLine();
            // Se o primeiro caracater da resposta é 1 a conexão foi feita com sucesso
            if (ConResposta[0] == '1')
            {
                // Atualiza o formulário para informar que esta conectado
                this.Invoke(new AtualizaLogCallBack(this.AtualizaLog), new object[] { "Conectado com sucesso!" });
            }
            else // Se o primeiro caractere não for 1 a conexão falhou
            {
                string Motivo = "Não Conectado: ";
                // Extrai o motivo da mensagem resposta. O motivo começa no 3o caractere
                Motivo += ConResposta.Substring(2, ConResposta.Length - 2);
                // Atualiza o formulário como o motivo da falha na conexão
                this.Invoke(new FechaConexaoCallBack(this.FechaConexao), new object[] { Motivo });
                // Sai do método
                return;
            }

            // Enquanto estiver conectado le as linhas que estão chegando do servidor
            while (Conectado)
            {
                // exibe mensagems no Textbox
                this.Invoke(new AtualizaLogCallBack(this.AtualizaLog), new object[] { strReceptor.ReadLine() });
            }
        }
        public void AtualizaLog(string strMensagem)
        {
            // Anexa texto ao final de cada linha
            txtLog.AppendText(strMensagem + "\r\n");
        }
        public void EnviaMensagem()
        {
            if (txtMensagem.Lines.Length >= 1)
            {
                stwEnviador.WriteLine(txtMensagem.Text);
                stwEnviador.Flush();
                txtMensagem.Lines = null;
            }
            txtMensagem.Text = "";
        }
        public void FechaConexao(string Motivo)
        {
            //FRONT
            //// Mostra o motivo porque a conexão encerrou
            //txtLog.AppendText(Motivo + "\r\n");
            //// Habilita e desabilita os controles apropriados no formulario
            //txtServidorIP.Enabled = true;
            //txtUsuario.Enabled = true;
            //txtMensagem.Enabled = false;
            //btnEnviar.Enabled = false;
            //btnConectar.Text = "Conectado";

            // Fecha os objetos
            Conectado = false;
            stwEnviador.Close();
            strReceptor.Close();
            tcpServidor.Close();
        }
    }
}
