import { Conta } from './Conta';

export class Historico {
        id: number;
        dataTransacao: Date;
        valor: number;
        descricaoTransacao:string
        tipoConta:string
        nomeConta:string
        digitosConta: number;
        digitosAgencia: number;
        digitosAgenciaDestino: number;
        digitosContaDestino: number;
        operacao: number;
        nomeContaDestino: string;
        contas: Conta[];
}