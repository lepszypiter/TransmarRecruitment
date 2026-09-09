import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-assembly-lines',
  templateUrl: './assembly-lines.component.html'
})
export class AssemblyLinesComponent implements OnInit {
  lines: any[] = [];
  products: any[] = [];
  name = '';
  productId: string | null = null;

  constructor(private api: ApiService) {}

  async ngOnInit() {
    await this.load();
  }

  async load() {
    this.products = await this.api.get('/api/products');
    this.lines = await this.api.get('/api/assembly-lines');
  }

  async create() {
    await this.api.post('/api/assembly-lines', { name: this.name, productId: this.productId });
    this.name = '';
    this.productId = null;
    await this.load();
  }

  async remove(id: string) {
    await this.api.delete('/api/assembly-lines/' + id);
    await this.load();
  }
}
