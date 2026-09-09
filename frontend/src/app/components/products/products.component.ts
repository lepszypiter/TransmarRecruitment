import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html'
})
export class ProductsComponent implements OnInit {
  products: any[] = [];
  name = '';
  error = '';

  constructor(private api: ApiService) {}

  async ngOnInit() {
    await this.load();
  }

  async load() {
    this.products = await this.api.get('/api/products');
  }

  async create() {
    try {
      await this.api.post('/api/products', { name: this.name });
      this.name = '';
      await this.load();
    } catch (err: any) {
      this.error = err.message || 'Create failed';
    }
  }

  async remove(id: string) {
    await this.api.delete('/api/products/' + id);
    await this.load();
  }
}
