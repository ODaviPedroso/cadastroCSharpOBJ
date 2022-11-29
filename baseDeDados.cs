using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace objetosUdemy
{
    [DataContract]
    internal class BaseDeDados
    {
        [DataMember]
        private List<CadastroPessoa> listaDePessoas;
        private string caminhoBaseDeDados;

        public void AdicionarPessoa(CadastroPessoa pPessoa)
        {
            listaDePessoas.Add(pPessoa);
            Serializador.Serializa(caminhoBaseDeDados, this);
        }
        public List<CadastroPessoa> PesquisaPessoaPorDoc(string pNumeroDeDocumento)
        {
            List<CadastroPessoa> ListaDePessoasTemp = listaDePessoas.Where(x => x.NumeroDoDocumento == pNumeroDeDocumento).ToList();
            if (ListaDePessoasTemp.Count > 0)
                return ListaDePessoasTemp;
            else
                return null;
        }
        public List<CadastroPessoa> RemoverPessoaPorDoc(string pNumeroDeDocumento)
        {
            List<CadastroPessoa> ListaDePessoasTemp = listaDePessoas.Where(x => x.NumeroDoDocumento == pNumeroDeDocumento).ToList();
            if (ListaDePessoasTemp.Count > 0)
            {
                foreach (CadastroPessoa pessoa in ListaDePessoasTemp)
                {
                    listaDePessoas.Remove(pessoa);
                }
                return ListaDePessoasTemp;
            }
            else
                return null;
        }

        public BaseDeDados(string pCaminhoBaseDeDados)
        {
            caminhoBaseDeDados = pCaminhoBaseDeDados;
            BaseDeDados baseDeDadostemp = Serializador.Desserializa(pCaminhoBaseDeDados);
            if (baseDeDadostemp != null)
                listaDePessoas = baseDeDadostemp.listaDePessoas;
            else
                listaDePessoas = new List<CadastroPessoa>();

        }
    }
}
