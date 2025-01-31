import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { HttpClientModule } from "@angular/common/http";
import { TooltipModule } from "ngx-bootstrap/tooltip";
import { ModalModule } from "ngx-bootstrap/modal";
import { BsDropdownModule } from "ngx-bootstrap/dropdown";
import { TabsModule } from "ngx-bootstrap/tabs";
import { AccordionModule } from "ngx-bootstrap/accordion";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { ReactiveFormsModule, FormsModule } from "@angular/forms";
import { NgxMaskModule, IConfig } from "ngx-mask";
import { NgxPaginationModule } from 'ngx-pagination';
import { NgxLoadingModule } from 'ngx-loading';
import { CollapseModule } from 'ngx-bootstrap/collapse';


import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { NavComponent } from "./nav/nav.component";
import { FooterComponent } from "./footer/footer.component";
import { ContaComponent } from "./conta/conta.component";
import { PessoaComponent } from "./pessoa/pessoa.component";
import { ToastrModule } from "ngx-toastr";
import { SolicitacaoComponent } from './solicitacao/solicitacao.component';
import { ContaSComponent } from './solicitacao/conta-s/conta-s.component';
import { ContaStatusComponent } from './solicitacao/conta-status/conta-status.component';
import { AdminComponent } from './admin/admin.component';
import { ContasComponent } from './admin/contas/contas.component';
import { PessoasComponent } from './admin/pessoas/pessoas.component';


@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    FooterComponent,
    ContaComponent,
    PessoaComponent,
    SolicitacaoComponent,
    ContaSComponent,
    ContaStatusComponent,
    AdminComponent,
    ContasComponent,
    PessoasComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    TooltipModule.forRoot(),
    ModalModule.forRoot(),
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    AccordionModule.forRoot(),
    BrowserAnimationsModule,
    ReactiveFormsModule,
    NgxMaskModule.forRoot(),
    FormsModule,
    CollapseModule.forRoot(),
    NgxPaginationModule,
    NgxLoadingModule.forRoot({}),
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: "toast-top-right",
      preventDuplicates: true,
    }),
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
