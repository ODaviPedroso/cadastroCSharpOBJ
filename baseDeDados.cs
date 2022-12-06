using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Threading;

namespace objetosUdemy
{
    [DataContract]
    internal class BaseDeDados
    {
        [DataMember]
        private List<CadastroPessoa> listaDePessoas;
        private string caminhoBaseDeDados;

        private Mutex mutexArquivo;
        private Mutex mutexLista;
        private bool baseDisponivel;

        public void AdicionarPessoa(CadastroPessoa pPessoa)
        {
            mutexArquivo.WaitOne();
            listaDePessoas.Add(pPessoa);
            mutexArquivo.ReleaseMutex();
            new Thread(() =>
            {
                baseDisponivel = false;
                mutexArquivo.WaitOne();
                Serializador.Serializa(caminhoBaseDeDados, this);
                mutexArquivo.ReleaseMutex();
                baseDisponivel = true;
            }).Start();
        }
        public List<CadastroPessoa> PesquisaPessoaPorDoc(string pNumeroDeDocumento)
        {
            mutexArquivo.WaitOne();
            List<CadastroPessoa> ListaDePessoasTemp = listaDePessoas.Where(x => x.NumeroDoDocumento == pNumeroDeDocumento).ToList();
            mutexArquivo.ReleaseMutex();
            if (ListaDePessoasTemp.Count > 0)
                return ListaDePessoasTemp;
            else
                return null;
        }
        public List<CadastroPessoa> RemoverPessoaPorDoc(string pNumeroDeDocumento)
        {
            mutexArquivo.WaitOne();
            List<CadastroPessoa> ListaDePessoasTemp = listaDePessoas.Where(x => x.NumeroDoDocumento == pNumeroDeDocumento).ToList();
            mutexArquivo.ReleaseMutex();
            if (ListaDePessoasTemp.Count > 0)
            {
                foreach (CadastroPessoa pessoa in ListaDePessoasTemp)
                {
                    mutexArquivo.WaitOne();
                    listaDePessoas.Remove(pessoa);
                    mutexArquivo.ReleaseMutex();
                }
                new Thread(() =>
                {
                    baseDisponivel = false;
                    mutexArquivo.WaitOne();
                    Serializador.Serializa(caminhoBaseDeDados, this);
                    mutexArquivo.ReleaseMutex();
                    baseDisponivel = true;

                }).Start();
                return ListaDePessoasTemp;
            }
            else
                return null;
        }

        public bool BaseDisponivel()
        {
            return baseDisponivel;
        }

        public BaseDeDados(string pCaminhoBaseDeDados)

        {

            caminhoBaseDeDados = pCaminhoBaseDeDados;

            mutexArquivo = new Mutex();
            mutexLista = new Mutex();
            baseDisponivel = true;

            new Thread(() =>
            {
                baseDisponivel = false;
                mutexArquivo.WaitOne();
                BaseDeDados baseDeDadostemp = Serializador.Desserializa(pCaminhoBaseDeDados);
                mutexArquivo.ReleaseMutex();

                mutexArquivo.WaitOne();
                if (baseDeDadostemp != null)
                    listaDePessoas = baseDeDadostemp.listaDePessoas;
                else
                    listaDePessoas = new List<CadastroPessoa>();
                mutexArquivo.ReleaseMutex();
                baseDisponivel = true;
            }).Start();

        }
    }
}
