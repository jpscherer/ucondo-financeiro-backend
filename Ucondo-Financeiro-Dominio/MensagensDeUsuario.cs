namespace Ucondo_Financeiro_Dominio
{
    public class MensagensDeUsuario
    {
        public static string MAIOR_CODIGO_DE_CONTA_UTILIZADO = "Maior código de conta já utilizado (999).";
        public static string CODIGO_MAXIMO_UTILIZADO_TODOS_NIVEIS = "Não foi possível sugerir próximo código. Todos os níveis estão com o código máximo cadastrado.";

        public static string CAMPO_NOME_OBRIGATORIO = "Campo nome é obrigatório!";
        public static string CAMPO_CODIGO_FORA_DO_INTERVALO_PERMITIDO = "Campo código somente é permitido entre os valores 0 e 999";

        public static string CONTA_POSSUI_REGISTROS_FILHOS = "Conta possui registros filhos";
        public static string NAO_E_POSSIVEL_ALTERAR_TIPO_CONTA_COM_FILHAS = "Não é possível alterar o tipo da conta quando há contas filhas";
        public static string NAO_ACEITA_LANCAMENTOS_QUANDO_POSSUI_FILHAS = "Não é permitido uma conta aceitar lançamentos quando possui contas filhas";
        public static string NAO_E_PERMITIDO_CONTA_FILHA_TIPO_DIFERENTE_CONTA_PAI = "Não é permitido cadastrar conta filha com tipo diferente que conta pai";
        public static string NAO_E_PERMITIDO_CADASTRAR_CONTA_FILHA_QUANDO_CONTA_PAI_ACEITA_LANCAMENTOS = "Não é permitido inserir conta filha para contas que aceitam lancamentos";

        public static string CODIGO_JA_CADASTRADO = "Código já cadastrado para o nível";

    }
}
