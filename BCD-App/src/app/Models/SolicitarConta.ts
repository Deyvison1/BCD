import { Endereco } from './Endereco';
import { EnumTipoConta } from './Enums/EnumTipoConta';

export class SolicitarConta {

    nomeConta: string;
    cpf: string;
    tipoConta: EnumTipoConta;
    senha: string;
    ceps: string[];
}