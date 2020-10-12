import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { ToastrService } from 'ngx-toastr';
import { Conta } from "src/app/Models/Conta";
import { Endereco } from "src/app/Models/Endereco";
import { ContaService } from "src/app/Services/ContaServices/conta.service";

@Component({
  selector: "app-contas",
  templateUrl: "./contas.component.html",
  styleUrls: ["./contas.component.css"],
})
export class ContasComponent implements OnInit {
  // PRIMITIVOS
  public loading = false;
  pageAtual = 1;
  // FORMS
  form: FormGroup;

  // ENTIDADEA
  conta: Conta = new Conta();
  endereco: Endereco = new Endereco();
  // LIST
  contas: Conta[] = [];
  contasFiltradas: Conta[] = [];

  _filtroLista = '';

  get filtroLista(): string {
    return this._filtroLista;
  }

  set filtroLista(value: string) {
    this._filtroLista = value;
    this.contasFiltradas = this._filtroLista
      ? this.filtrarLista(this.filtroLista)
      : this.contas;
  }

  constructor(private contaService: ContaService, private fb: FormBuilder,
      private toastr: ToastrService
    ) {}

  ngOnInit() {
    this.getAll();
    this.validation();
  }

  filtrarLista(search: string): Conta[] {
    this.loading = true;
    this.contaService.getBySearch(search).subscribe(
      (contasFiltradas: Conta[]) => {
        this.contasFiltradas = contasFiltradas;
        this.loading = false;
        return this.contas;
      },
      (error) => {
        this.loading = false;
        console.log(error.error);
      }
    );
    this.loading = false;
    return [];
  }

  abrirModal(template: any) {
    template.show();
  }

  editar(template: any, conta: Conta) {
    this.abrirModal(template);

    this.toastr.warning(`Modo Edição Para ${conta.nomeConta}`);
    this.conta = conta;
    this.form.patchValue(this.conta);
  }

  detalhes(template: any, conta: Conta) {
    this.abrirModal(template);
    this.toastr.info(`Modo Detalhes Para ${conta.nomeConta}`);

    this.contaService.getById(conta.id).subscribe(
      (conta: Conta) => {
        this.conta = conta;
        this.form.patchValue(this.conta);
      },
      (error) => {
        this.toastr.error(error.error);
      }
    );
  }

  deletar(template: any, conta: Conta) {
    this.abrirModal(template);

    this.conta = Object.assign({  }, conta);
    this.toastr.warning(`Modo Deletar Para ${conta.nomeConta}`);
  }

  confirmDelete(template: any) {
    this.contaService.delete(this.conta.id).subscribe(
      (conta: Conta) => {
        this.conta = conta;
        this.toastr.success(`Sucesso ao Deletar ${this.conta.nomeConta}`);
        this.getAll();
        template.hide();
      }, error => {
        console.log(error.error);
      }
    );
  }

  salvarAlteracao(template: any) {
    this.loading = true;
    this.conta = Object.assign({}, this.form.value);

    this.contaService.put(this.conta).subscribe(
      (conta: Conta) => {
        this.conta = conta;
        this.toastr.success('Sucesso no Editar');
        this.getAll();
        template.hide();
        this.loading = false;
      }, error => { console.log(error.error); this.loading = false; }
    );
  }

  getAll() {
    return this.contaService.getAll().subscribe(
      (contas: Conta[]) => {
        this.contas = contas;

        this.contasFiltradas = this.contas;
      },
      (err) => {
        console.log(err.error);
      }
    );
  }

  validation() {
    this.form = this.fb.group({
      id: [""],
      digitosConta: [""],
      digitosAgencia: [""],
      tipoConta: [""],
      nomeConta: [""],
      saldo: [""],
      pessoaId: [""],
      senha: [""],
      cpf: [""],
      situacao: [""],
    });
  }
}
