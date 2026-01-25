# 🏦 Exercício de Desenvolvimento de Software: Simulador de ATM

## 🎯 Objetivo
Desenvolver um simulador de caixa eletrônico (ATM) que permita realizar **saques de valores** utilizando apenas três tipos de notas: **R$ 20, R$ 50 e R$ 100**.  
O exercício tem como propósito treinar conceitos de **Orientação a Objetos (OO)**, como **encapsulamento**, **abstração**, **responsabilidade das classes** e **interação entre objetos**.

---

## 📜 Regras de Negócio
1. O ATM possui **3 slots de notas**:
   - Slot de R$ 20  
   - Slot de R$ 50  
   - Slot de R$ 100  

2. O ATM deve possuir um **comando de setup** para configurar a quantidade inicial de notas em cada slot.

3. Ao receber um pedido de saque:
   - O sistema deve calcular **até 3 opções diferentes** de distribuição de notas que atendam ao valor solicitado.  
   - Caso não seja possível atender ao valor com as notas disponíveis, o sistema deve informar que o saque não pode ser realizado.

4. O usuário deve **selecionar uma das opções sugeridas**.  
   - Após a seleção, o sistema deve **debitar as notas correspondentes** do estoque dos slots.

5. O ATM deve sempre respeitar o estoque atual de notas.  
   - Exemplo: se o slot de R$ 20 estiver zerado, nenhuma opção pode incluir notas de R$ 20.

---

## ⚙️ Aspectos Funcionais
- **Setup inicial**  
  - Método para configurar a quantidade de notas em cada slot.  
  - Exemplo: `setup(20: 10, 50: 5, 100: 2)`.

- **Solicitação de saque**  
  - Método que recebe o valor solicitado.  
  - Exemplo: `sacar(150)`.

- **Cálculo de opções**  
  - Algoritmo que gera até 3 combinações possíveis de notas.  
  - Exemplo: Para R$ 150, com estoque de 3 notas de R$ 50 e 2 notas de R$ 100:  
    - Opção 1: 3 × R$ 50  
    - Opção 2: 1 × R$ 100 + 1 × R$ 50  

- **Seleção da opção**  
  - Usuário escolhe uma das opções sugeridas.  
  - O sistema atualiza o estoque de notas.

- **Controle de estoque**  
  - Após cada saque, o ATM deve refletir corretamente o número de notas restantes.

---

## 🧩 Expectativa Final
Ao concluir o exercício, o aluno deverá ter:
- Um conjunto de **classes bem definidas**, por exemplo:
  - `ATM` (controla o fluxo principal)  
  - `Slot` (representa cada compartimento de notas)  
  - `Saque` (representa uma operação de saque)  
  - `DistribuicaoNotas` (representa uma opção de combinação de notas)

- Métodos que demonstrem:
  - **Encapsulamento** (cada classe cuida de sua própria responsabilidade).  
  - **Abstração** (o usuário não precisa conhecer os detalhes internos do cálculo).  
  - **Interação entre objetos** (o ATM consulta os slots, gera opções e atualiza o estoque).  

- Um programa capaz de:
  - Configurar o ATM.  
  - Receber pedidos de saque.  
  - Oferecer até 3 opções de distribuição de notas.  
  - Atualizar corretamente o estoque após a escolha.

---

## 🚀 Sugestões de Expansão
- Relatório de estoque atual.  
- Registro de histórico de saques.  
- Tratamento de exceções (ex: valor não múltiplo de 10 ou 20).  


---

 ## 📖 Documentação Completa da Solução
 
 - [🎯 Link](SOLUTION_DOCUMENTATION.md)

---