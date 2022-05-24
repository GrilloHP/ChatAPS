using ChatAPS.View;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChatAPS.Controler
{
    public class conexao
    {
        // Trata o nome do usuário
        public StreamWriter stwEnviador;
        public StreamReader strReceptor;
        public TcpClient tcpServidor;
        public Thread mensagemThread;
        public IPAddress enderecoIP;

        public bool InicializaConexao(string servidorIP, string usuario = "Desconhecido")
        {
            try
            {
                // Trata o endereço IP informado em um objeto IPAdress
                enderecoIP = IPAddress.Parse(servidorIP);
                // Inicia uma nova conexão TCP com o servidor chat
                tcpServidor = new TcpClient();
                tcpServidor.Connect(enderecoIP, 2502);

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
            }
        }

        public void AtualizaLog(string strMensagem)
        {
            ChatView frm = new ChatView();
            // Anexa texto ao final de cada linha
            frm.txtChat.AppendText(strMensagem + "\r\n");
        }
        public void EnviaMensagem(string mensagem)
        {
            if (mensagem.Length >= 1)
            {
                stwEnviador.WriteLine(mensagem);
                stwEnviador.Flush();
            }
        }
        public void FechaConexao(string Motivo)
        {
            stwEnviador.Close();
            strReceptor.Close();
            tcpServidor.Close();
            LoginView fmr = new LoginView();
            fmr.Conectado = false;
        }
        public void RecebeMensagens()
        {
            // recebe a resposta do servidor
            strReceptor = new StreamReader(this.tcpServidor.GetStream());
            LoginView frm = new LoginView();
            string ConResposta = strReceptor.ReadLine();
            // Se o primeiro caracater da resposta é 1 a conexão foi feita com sucesso
            if (ConResposta[0] == '1')
            {
                AtualizaLog("Conectado com sucesso!");               
            }
            else // Se o primeiro caractere não for 1 a conexão falhou
            {
                string Motivo = "Não Conectado: ";
                // Extrai o motivo da mensagem resposta. O motivo começa no 3o caractere
                Motivo += ConResposta.Substring(2, ConResposta.Length - 2);
                // Atualiza o formulário como o motivo da falha na conexão
                FechaConexao(Motivo);
                // Sai do método
            }

            // Enquanto estiver conectado le as linhas que estão chegando do servidor
            while (frm.Conectado)
            {
                AtualizaLog(strReceptor.ReadLine());
            }
        }
    }
}
