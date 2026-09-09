import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { DragDropModule } from '@angular/cdk/drag-drop';

import { AppComponent } from './app.component';
import { ProductsComponent } from './components/products/products.component';
import { AssemblyLinesComponent } from './components/assembly-lines/assembly-lines.component';
import { WorkstationsComponent } from './components/workstations/workstations.component';
import { AllocationComponent } from './components/allocation/allocation.component';

@NgModule({
  declarations: [
    AppComponent,
    ProductsComponent,
    AssemblyLinesComponent,
    WorkstationsComponent,
    AllocationComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    DragDropModule,
    RouterModule.forRoot([
      { path: '', redirectTo: 'products', pathMatch: 'full' },
      { path: 'products', component: ProductsComponent },
      { path: 'assembly-lines', component: AssemblyLinesComponent },
      { path: 'workstations', component: WorkstationsComponent },
      { path: 'assembly-lines/:id/allocations', component: AllocationComponent }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
