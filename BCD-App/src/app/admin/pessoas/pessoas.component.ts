import { Component, OnInit } from "@angular/core";
import { FormArray, FormBuilder, FormGroup } from "@angular/forms";
import { ToastrService } from "ngx-toastr";
import { Conta } from "src/app/Models/Conta";
import { Endereco } from "src/app/Models/Endereco";
import { Pessoa } from "src/app/Models/Pessoa";
import { EnderecoService } from "src/app/Services/EnderecoServices/endereco.service";
import { PessoaService } from "src/app/Services/PessoaServices/pessoa.service";

@Component({
  selector: "app-pessoas",
  templateUrl: "./pessoas.component.html",
  styleUrls: ["./pessoas.component.css"],
})
export class PessoasComponent implements OnInit {
  // LOADING PAGE
  public loading = false;
  // PRIMITIVOS
  pageAtual = 1;
  cep: number;
  _filtroLista = '';


  // FORMS
  form: FormGroup;

  // ENTIDADES
  pessoa: Pessoa = new Pessoa();
  endereco: Endereco = new Endereco();
  conta: Conta = new Conta();
  // LISTAS
  pessoas: Pessoa[] = [];
  pessoasFiltradas: Pessoa[] = [];

  get filtroLista(): string {
    return this._filtroLista;
  }

  set filtroLista(value: string) {
    this._filtroLista = value;
    this.pessoasFiltradas = this._filtroLista? this.filtrarLista(this.filtroLista) : this.pessoas;
  }

  get enderecos(): FormArray {
    return <FormArray>this.form.get("enderecos");
  }

  constructor(
    private pessoaService: PessoaService,
    private fb: FormBuilder,
    private enderecoService: EnderecoService,
    private toastr: ToastrService
  ) {}

  ngOnInit() {
    this.validation();
    this.getAll();
  }
  // FILTRAR -----------------------------------------------------------------------------
  filtrarLista(search: string): Pessoa[] {
    this.loading = true;
    this.pessoaService.getSearchNomeOrCpf(search).subscribe(
      (pessoasFiltradas: Pessoa[]) => {
        this.pessoasFiltradas = pessoasFiltradas;
        this.loading = false;
        return this.pessoas;
      }, error => { console.log(error.error); }
    );
    this.loading = false;
    return [];
  }

  // ACOES ---------------------------------------------------------------------------
  abrirModal(template: any) {
    template.show();
  }

  removerItemList(id: number) {
    this.enderecos.removeAt(id);
  }

  editar(pessoa: Pessoa, template: any) {
    this.abrirModal(template);

    this.pessoaService.getById(pessoa.id).subscribe(
      (pessoa: Pessoa) => {
        this.pessoa = Object.assign({}, pessoa);

        this.form.patchValue(this.pessoa);

        this.pessoa.enderecos.forEach((enderecos) => {
          this.enderecos.push(this.criarEndereco(enderecos));
        });
      },
      (error) => {
        console.log(error.errror);
      }
    );
  }

  // REQUISICOES GET -----------------------------------------------------------------
  getAll() {
    return this.pessoaService.getAll().subscribe(
      (pessoas: Pessoa[]) => {
        this.pessoas = pessoas;

        this.pessoasFiltradas = this.pessoas;
      },
      (err) => {
        console.log(err.error);
      }
    );
  }
  // Novo Endereco ----------------------------------------------------------------------------------------
  addConta(conta: Conta) {
    console.log(conta);
  }  

  // Novo Endereco ----------------------------------------------------------------------------------------

  abrirModalNewEndereco(template: any) {
    this.abrirModal(template);
  }

  addEndereco(template: any) {
    this.enderecoService.getEnderecoByCep(this.cep).subscribe(
      (endereco) => {
        this.enderecos.push(this.criarEndereco(endereco));
        this.toastr.success('Sucesso Ao Adicionar Novo Cep');
        template.hide();
      },
      (error) => {
        console.log(error);
      }
    );
  }

  criarEndereco(endereco: any) {
    return this.fb.group({
      id: [endereco.id],
      cep: [endereco.cep],
      logradouro: [endereco.logradouro],
      bairro: [endereco.bairro],
      localidade: [endereco.localidade],
      uf: [endereco.uf],
      ibge: [endereco.ibge],
    });
  }

  // SALVAR --------------------------------------------------------------------------
  salvar() {
    this.pessoa = Object.assign({id: this.pessoa.id}, this.form.value);
    console.log(this.pessoa);
  }
  // VALIDAÇÃO ------------------------------------------------------------------------
  validation() {
    this.form = this.fb.group({
      id: [],
      nome: [],
      cpf: [],
      situacao: [],
      enderecos: this.fb.array([]),
    });
  }
}
