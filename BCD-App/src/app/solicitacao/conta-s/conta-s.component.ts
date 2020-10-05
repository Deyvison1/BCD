import { Component, OnInit } from "@angular/core";
import { FormArray, FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ToastrService } from "ngx-toastr";
import { Conta } from "src/app/Models/Conta";
import { Endereco } from "src/app/Models/Endereco";
import { Pessoa } from "src/app/Models/Pessoa";
import { SolicitarConta } from "src/app/Models/SolicitarConta";
import { ContaService } from "src/app/Services/ContaServices/conta.service";
import { EnderecoService } from "src/app/Services/EnderecoServices/endereco.service";

@Component({
  selector: "app-conta-s",
  templateUrl: "./conta-s.component.html",
  styleUrls: ["./conta-s.component.css"],
})
export class ContaSComponent implements OnInit {
  public loading = false;
  // VARIAEVEIS TIPO PRIMITIVO
  isCollapsed = true;

  // FormGroup
  form: FormGroup;

  // VARIAVEIS DE INSTANCIAS
  solicitarConta: SolicitarConta = new SolicitarConta();
  solicitarConta2: SolicitarConta = new SolicitarConta();
  enderecoInstancia: Endereco = new Endereco();

  // LISTAS
  enderecosCompletos: Endereco[] = [];
  contas: Conta[] = [];

  // FormArray
  get ceps(): FormArray {
    return <FormArray>this.form.get("ceps");
  }

  constructor(
    private fb: FormBuilder,
    private enderecoService: EnderecoService,
    private toastr: ToastrService,
    private contaService: ContaService
  ) {}

  ngOnInit() {
    this.validation();
  }

  solicitar() {
    this.solicitarConta = Object.assign({}, this.form.value);

    this.contaService.addSolicitacao(this.solicitarConta).subscribe(
      (data) => {
        this.toastr.success('Sucesso na Solicitação!');
        this.form.reset();
      }, err => { console.log(err.error); }
    );
  }

  addEndereco() {
    this.ceps.push(this.criarCep({ id: 0 }));
  }

  removerEndereco(id: number) {
    this.ceps.removeAt(id);
  }

  criarCep(cep: any) {
    return this.fb.group({
      cep: [cep.cep]
    });
  }

  validation() {
    this.form = this.fb.group({
      tipoConta: [0],
      senha: [''],
      nomeConta: [''],
      cpf: [''],
      ceps: this.fb.array([]),
    });
  }
}
