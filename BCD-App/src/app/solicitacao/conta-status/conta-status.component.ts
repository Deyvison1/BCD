import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ContaService } from 'src/app/Services/ContaServices/conta.service';

@Component({
  selector: 'app-conta-status',
  templateUrl: './conta-status.component.html',
  styleUrls: ['./conta-status.component.css']
})
export class ContaStatusComponent implements OnInit {

  model: any = {};
  situacao: number;
  resultado = false;
  
  constructor(
    private contaService: ContaService,
    private toastr: ToastrService,
    private router: Router
  ) { }

  ngOnInit() {
  }


  checkStatus() {
    this.contaService.statusSolicitacao(this.model).subscribe(
      (data: number) => {
        this.situacao = data;
        this.resultado = true;
      }, error => { this.toastr.error(error.error); }
    );
  }
}
