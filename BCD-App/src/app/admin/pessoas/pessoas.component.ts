import { Component, OnInit, TemplateRef } from "@angular/core";
import { FormArray, FormBuilder, FormGroup } from "@angular/forms";
import { ToastrService } from "ngx-toastr";
import { Conta } from "src/app/Models/Conta";
import { Endereco } from "src/app/Models/Endereco";
import { Pessoa } from "src/app/Models/Pessoa";
import { EnderecoService } from "src/app/Services/EnderecoServices/endereco.service";
import { PessoaService } from "src/app/Services/PessoaServices/pessoa.service";
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';


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
  _filtroLista = "";

  // NGX-BOOTSTRAP
  modalRef: BsModalRef;

  // FORMS
  form: FormGroup;
  formEndereco: FormGroup;

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
    this.pessoasFiltradas = this._filtroLista
      ? this.filtrarLista(this.filtroLista)
      : this.pessoas;
  }

  get enderecos(): FormArray {
    return <FormArray>this.form.get("enderecos");
  }

  constructor(
    private pessoaService: PessoaService,
    private fb: FormBuilder,
    private enderecoService: EnderecoService,
    private toastr: ToastrService,
    private modalService: BsModalService 
  ) {}

  ngOnInit() {
    this.validationEndereco();
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
      },
      (error) => {
        console.log(error.error);
      }
    );
    this.loading = false;
    return [];
  }

  // ACOES ---------------------------------------------------------------------------
  abrirModal(template: any) {
    template.show();
  }

  abrirModalService(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirm() {
    this.toastr.warning('Vamos Deletar');
  }

  decline() {
    this.toastr.info('Voce saiu fora');
  }

  details(template: any, pessoa: Pessoa) {
    this.abrirModal(template);
    this.toastr.info(`Detalhes: ${pessoa.nome.toUpperCase()}`);

    this.enderecoService.getById(pessoa.enderecoId).subscribe(
      (endereco: Endereco) => {
        this.endereco = endereco;
      }, error => { console.log(error.error); }
    );
    this.pessoa = pessoa;
  }

  // MODAL EDITAR
  editar(pessoa: Pessoa, template: any) {
    this.abrirModal(template);

    this.toastr.warning(`Modo Edição Para ${pessoa.nome}`);
    // PEGAR A PESSOA PELO ID
    this.pessoaService.getById(pessoa.id).subscribe(
      (pessoa: Pessoa) => {
        this.pessoa = Object.assign({}, pessoa);

        // PEGAR O ENDERECO PELO ID
        this.enderecoService.getById(pessoa.enderecoId).subscribe(
          (endereco: Endereco) => {
            this.endereco = endereco;
            this.formEndereco.patchValue(this.endereco);
          },
          (error) => {
            console.log(error.error);
          }
        );
        this.form.patchValue(this.pessoa);
      },
      (error) => {
        console.log(error.errror);
      }
    );
  }

  // REQUISICOES GET -----------------------------------------------------------------
  getAll() {
    this.loading = true;

    return this.pessoaService.getAll().subscribe(
      (pessoas: Pessoa[]) => {
        this.pessoas = pessoas;

        this.loading = false;
        this.pessoasFiltradas = this.pessoas;
      },
      (err) => {
        this.loading = false;
        console.log(err.error);
      }
    );
  }


  // Novo Endereco ----------------------------------------------------------------------------------------
  abrirModalNewEndereco(template: any) {
    this.abrirModal(template);
  }

  // SALVAR --------------------------------------------------------------------------
  salvar(template: any) {
    this.loading = true;
    this.pessoa = Object.assign({ id: this.pessoa.id }, this.form.value);

    this.pessoaService.update(this.pessoa).subscribe(
      (pessoa: Pessoa) => {
        this.pessoa = pessoa;

        this.enderecoService.getEnderecoByCep(this.endereco.cep).subscribe(
          (endereco: Endereco) => {

            if(endereco.cep == null) {
              this.toastr.error('Cep Invalido');
              this.loading = false;
              return ;
            }
            this.endereco = Object.assign({ id: this.endereco.id }, endereco);

            this.enderecoService.update(this.endereco).subscribe(
              (endereco: Endereco) => {
                this.endereco = endereco;
    
    
                this.toastr.success("Edição com Sucesso!");
                this.getAll();
                template.hide();
                this.loading = false;
              },
              (error) => {
                console.log(error.error);
                this.loading = false;
              }
            );
          },
          (err) => {
            this.loading = false;
            console.log(err.error);
          }
        );
      },
      (error) => {
        this.toastr.error('Cep Invalido!');
        this.loading = false;
      }
    );
  }

  // VALIDAÇÃO ------------------------------------------------------------------------
  validation() {
    this.form = this.fb.group({
      id: [],
      nome: [],
      cpf: [],
      situacao: [],
      enderecoId: [""],
    });
  }

  validationEndereco() {
    this.formEndereco = this.fb.group({
      id: [""],
      cep: [""],
      logradouro: [""],
      bairro: [""],
      localidade: [""],
      uf: [""],
      ibge: [""],
    });
  }
}
