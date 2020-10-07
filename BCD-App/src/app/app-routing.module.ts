import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AdminComponent } from './admin/admin.component';
import { ContasComponent } from './admin/contas/contas.component';
import { PessoasComponent } from './admin/pessoas/pessoas.component';
import { ContaComponent } from "./conta/conta.component";
import { ContaSComponent } from './solicitacao/conta-s/conta-s.component';
import { ContaStatusComponent } from './solicitacao/conta-status/conta-status.component';
import { SolicitacaoComponent } from "./solicitacao/solicitacao.component";

const routes: Routes = [
  { path: 'conta', component: ContaComponent },
  { path: 'solicitacao', component: SolicitacaoComponent, children: 
  [
    { path: 'conta', component: ContaSComponent },
    { path: 'status', component: ContaStatusComponent },
    { path: '', redirectTo: 'conta', pathMatch: 'full' },
    { path: '**', redirectTo: 'conta', pathMatch: 'full' }
  ] },
  { path: 'admin', component: AdminComponent, children: 
  [
    { path: 'contas', component: ContasComponent },
    { path: 'pessoas', component: PessoasComponent},
    { path: '', redirectTo: 'contas', pathMatch: 'full' },
    { path: '**', redirectTo: 'contas', pathMatch: 'full' } 
  ]  
  },
  { path: '', redirectTo: 'conta', pathMatch: 'full' },
  { path: '**', redirectTo: 'conta', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
