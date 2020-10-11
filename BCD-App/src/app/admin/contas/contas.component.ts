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

  constructor(private contaService: ContaService, private fb: FormBuilder,
      private toastr: ToastrService
    ) {}

  ngOnInit() {
    this.getAll();
    this.validation();
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

    this.contaService.getById(conta.id).subscribe(
      (conta: Conta) => {
        this.conta = conta;
        this.form.patchValue(this.conta);
      },
      (error) => {
        console.log(error.error);
      }
    );
  }

  delete() {}

  confirmDelete(template: any) {}

  salvarAlteracao(template: any) {
    this.loading = true;
    this.conta = Object.assign({}, this.form.value);

    this.contaService.put(this.conta).subscribe(
      (conta: Conta) => {
        this.conta = conta;
        this.toastr.success('Sucesso no Editar');
        template.hide();
        this.loading = false;
      }, error => { console.log(error.error); this.loading = false; }
    );
  }

  getAll() {
    return this.contaService.getAll().subscribe(
      (contas: Conta[]) => {
        this.contas = contas;
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
