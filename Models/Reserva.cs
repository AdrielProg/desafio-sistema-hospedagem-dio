using DesafioProjetoHospedagem.Exception;

namespace DesafioProjetoHospedagem.Models
{
    public class Reserva
    {
        public List<Pessoa> Hospedes { get; set; }
        public Suite Suite { get; set; }
        public int DiasReservados { get; set; }

        public Reserva() { }

        public Reserva(int diasReservados)
        {
            DiasReservados = diasReservados;
        }


        public List<Pessoa> CadastrarHospedes(List<Pessoa> hospedes)
        {
            Console.WriteLine("Quantos hospedes para essa reserva ? ");
            try
            {
                string quantidadeHospedes = Console.ReadLine();

                if (!int.TryParse(quantidadeHospedes, out int i))
                {
                    Console.WriteLine("Quantidade invalida");
                    return null;
                }

                hospedes = MenuInformacoesCadastrais(hospedes, i);

                if (Suite.Capacidade >= hospedes.Count)
                {
                    Hospedes = hospedes;
                }
                else
                {
                    throw new CapacidadeExcedidaException();
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new CapacidadeInvalidaException();
            }
            return hospedes;
        }

        public int CadastrarSuite(Suite suite)
        {
            Suite = suite;
            return suite.Id;
        }

        public int ObterQuantidadeHospedes()
        {
            return Hospedes.Any() ? Hospedes.Count : 0;
        }

        public decimal CalcularValorDiaria()
        {
            decimal valor = 0;

            valor = DiasReservados * Suite.ValorDiaria;

            if (DiasReservados >= 10)
            {
                valor *= 0.9M;
            }

            return valor;
        }

        public static List<Pessoa> MenuInformacoesCadastrais(List<Pessoa> hospedes, int quantidadeHospedes)
        {
            int contador;
            Console.WriteLine("Cadastro de Hospede\n");
            for (contador = 1; contador <= quantidadeHospedes; contador++)
            {
                Console.Write($"Hospede: {contador}°");
                Console.Write("Nome: ");
                string nome = Console.ReadLine();
                Console.Write("Sobrenome: ");
                string sobrenome = Console.ReadLine();

                Pessoa hospede = Pessoa.CriarPessoa(nome, sobrenome);
                hospedes.Add(hospede);
            }
            return hospedes;
        }

        public static Reserva CriarReserva(int diasReservados)
        {
            return new Reserva(diasReservados);
        }

        public string GerarComprovanteReserva()
        {
            string hospedesInfo = Hospedes != null && Hospedes.Count > 0
                ? string.Join(", ", Hospedes.ConvertAll(h => h.Nome))
                : "Nenhum hóspede registrado";

            string suiteInfo = Suite != null
                ? Suite.GerarDescricaoSuite()
                : "Nenhuma suíte selecionada";

            return $@"
            Descrição da Reserva:
            ----------------------
            Hóspedes: {hospedesInfo}
            Suíte: {suiteInfo}
            Dias Reservados: {DiasReservados}";

        }

    }
}