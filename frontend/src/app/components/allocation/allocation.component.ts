import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-allocation',
  templateUrl: './allocation.component.html'
})
export class AllocationComponent implements OnInit {
  assemblyLineId: string = '';
  allocations: any[] = [];
  available: any[] = [];
  selectedIds: string[] = [];

  constructor(private route: ActivatedRoute, private api: ApiService) {}

  async ngOnInit() {
    this.assemblyLineId = this.route.snapshot.paramMap.get('id') || '';
    await this.load();
  }

  async load() {
    this.allocations = await this.api.get(`/api/assembly-lines/${this.assemblyLineId}/workstations`);
    this.available = await this.api.get('/api/workstations');
  }

  select(id: string, checked: any) {
    if (checked) this.selectedIds.push(id);
    else this.selectedIds = this.selectedIds.filter(x => x !== id);
  }

  async addSelected() {
    if (!this.selectedIds.length) return;
    await this.api.post(`/api/assembly-lines/${this.assemblyLineId}/workstations`, { workstationIds: this.selectedIds });
    this.selectedIds = [];
    await this.load();
  }

  async remove(allocationId: string) {
    await this.api.delete(`/api/assembly-lines/${this.assemblyLineId}/workstations/${allocationId}`);
    await this.load();
  }

  drop(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.allocations, event.previousIndex, event.currentIndex);
    // send order of allocation ids to backend
    const order = this.allocations.map(a => a.id);
    this.api.put(`/api/assembly-lines/${this.assemblyLineId}/workstations/reorder`, { order });
  }
}
