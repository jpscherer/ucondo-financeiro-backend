# Execução
1 - É necessário rodar as migrations. A string de conexão está hardcoded na classe FinanceiroDbContext.cs, linha 8, Necessário ajustar para o banco disponível/em uso.
2 - Com o projeto Ucondo-Financeiro-Data selecionado, rodar o comando Update-Database na janela 'Package Manager Console'. (ou alguma outra tecnica de execução das migrations)
3 - Selecionar como projeto de startup Ucondo-Financeiro-Api, buildar a solucão e executar.
4 - A página padrão será a página do swagger onde está contido todos os endpoints disponíveis

# Sobre as listagens
Para as listagens foram oferecidos 3 endpoints (1 - HTTP Get / ; 2 - HTTP Get /ByContaPai/{idContaPai} ; 3 - HTTP Get /{pesquisa}) com propósitos distintos.
1 - Retorna somente as contas Raiz (para a listagem inicial);
2 - Retorna somente as contas filhas da conta raiz informada (para retornar as contas da conta raiz "aberta")
3 - Retorna todas as contas da base, filtrando pela pesquisa passada nos campos código e nome da conta (para a listagem inicial quando filtrado).

(para economia de banda)

# Sobre endpoint próximo-código
É necessário passar o id da conta raiz onde procurasse o próximo código exemplo:

10.? = passasse o id da conta com código 10.
10.5.? = passasse o id da conta com código 5.
? = passasse o Guid.Empty (00000000-0000-0000-0000-000000000000)

Do retorno:
{
  idContaPai = id da conta pai do código sugerido (podendo ser o mesmo passado por parâmetro, como outro id de conta pai, como Guid.Empty quando se torna conta Raiz)
  Codigo = novo código sugerido
  ContaPaiAlterado = se a conta pai foi alterada para um novo código disponível.
}
