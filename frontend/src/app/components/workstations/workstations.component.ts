import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-workstations',
  templateUrl: './workstations.component.html'
})
export class WorkstationsComponent implements OnInit {
  workstations: any[] = [];
  short_name = '';
  name = '';
  pc_name = '';

  constructor(private api: ApiService) {}

  async ngOnInit() {
    await this.load();
  }

  async load() {
    this.workstations = await this.api.get('/api/workstations');
  }

  async create() {
    await this.api.post('/api/workstations', { short_name: this.short_name, name: this.name, pc_name: this.pc_name });
    this.short_name = ''; this.name = ''; this.pc_name = '';
    await this.load();
  }

  async remove(id: string) {
    await this.api.delete('/api/workstations/' + id);
    await this.load();
  }
}
