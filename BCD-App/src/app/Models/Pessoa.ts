import { Endereco } from './Endereco'
import { Conta } from './Conta'

export class Pessoa {
        id: number;
        nome: string;
        cpf: string;
        // 0 -> Ativada, 1 -> Desativada, 2 -> Bloqueada, 3 -> Analise
        situacao: number;
        contas: Conta[];
        enderecos: Endereco[];
}