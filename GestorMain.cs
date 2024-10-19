using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace GestorDeClientes
{
    internal class Program
    {
        [System.Serializable] // permite guardar em arquivos binários, formatando p/ bytes
        struct Cliente // struct para criar os objetos do tipo cliente
        {
            public string nome;
            public string email;
            public string cpf;
        }

        static List<Cliente> clientes = new List<Cliente>();

        enum Menu { Listagem = 1, Adicionar, Remover, Sair } // novo tipo Menu 
        static void Main(string[] args)
        {
            Carregar(); //Vai carregar os dados salvos em arquivo do programa 

            bool escolheuSair = false;

            while (!escolheuSair) 
            {
                Console.Clear();
                Console.WriteLine("Sistema de Clientes");
                Console.WriteLine("1 - Listagem\n2 - Adicionar\n3 - Remover\n4 - Sair");

                Menu escolha = (Menu)int.Parse(Console.ReadLine());

                Console.Clear();

                switch (escolha)
                {
                    case Menu.Listagem:
                        Listagem();
                        break;
                    case Menu.Adicionar:
                        Adicionar();
                        break;
                    case Menu.Remover:
                        Remover();  
                        break;
                    case Menu.Sair:
                        escolheuSair = true;
                        break;
                    default:
                        Console.WriteLine("Nenhuma Opcao Valida");
                        break;
                }
            }
            Console.ReadLine();
        }
        
        static void Adicionar()
        {
            Cliente cliente = new Cliente();
            Console.WriteLine("Cadastro de cliente: ");
            Console.Write("Nome do cliente: ");
            cliente.nome = Console.ReadLine();
            Console.Write("Email do cliente: ");
            cliente.email = Console.ReadLine();
            Console.Write("CPF do cliente: ");
            cliente.cpf = Console.ReadLine();
            Console.WriteLine("\nCadastro Concluído\nAperte Enter para sair.");
            Console.ReadLine();

            clientes.Add(cliente);
            Salvar();
        }

        static void Listagem()
        {
            if(clientes.Count > 0)
            {
                int id = 1;
                Console.WriteLine("Lista de Clientes:");
                foreach (Cliente cliente in clientes)
                {
                    Console.WriteLine($"ID: {id}");
                    Console.WriteLine($"Nome: {cliente.nome}");
                    Console.WriteLine($"Email: {cliente.email}");
                    Console.WriteLine($"CPF: {cliente.cpf}");
                    id++;
                    Console.WriteLine("===============================");
                }
            } else { Console.WriteLine("Nenhum Cliente Cadastrado"); }
            
            
            
            

            Console.WriteLine("\nAperte Enter para voltar ao menu");
            Console.ReadLine();
        }

        static void Remover()
        {
            Listagem();
            Console.WriteLine("Digite o ID do cliente para remover");
            int id = int.Parse(Console.ReadLine());

            if(id > 0 && id < clientes.Count)
            {
                clientes.RemoveAt(id - 1);
                Salvar();
            }else
            {
                Console.WriteLine("ID inválido, tente novamente");
                Console.ReadLine();
            }
        }

        static void Salvar()
        {
            FileStream stream = new FileStream("clientes.dados",FileMode.OpenOrCreate);
            BinaryFormatter encoder = new BinaryFormatter();

            encoder.Serialize(stream, clientes);

            stream.Close();
        }

        static void Carregar()
        {
            FileStream stream = new FileStream("clientes.dados", FileMode.OpenOrCreate);
            try
            {
                BinaryFormatter encoder = new BinaryFormatter();

                clientes = (List<Cliente>)encoder.Deserialize(stream);

                if(clientes == null) //Caso retorne uma lista nula
                {
                    clientes = new List<Cliente>();
                }
            }
            catch(Exception e) // caso haja um erro
            {
                clientes = new List<Cliente>();
            }

            stream.Close();
        }
    }
}
