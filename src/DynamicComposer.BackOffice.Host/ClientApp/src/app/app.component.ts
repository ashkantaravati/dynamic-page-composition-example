import { CdkDragDrop, DragDropModule, moveItemInArray } from '@angular/cdk/drag-drop';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';

interface PageSummaryDto { id: string; title: string; slug: string; description?: string; }
interface MetaDataDto { title?: string; description?: string; keywords?: string; }
interface BlockDto { id: string; sortOrder: number; type: string; heading?: string; body?: string; }

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule, DragDropModule],
  templateUrl: './app.component.html'
})
export class AppComponent {
  private readonly http = inject(HttpClient);
  pages: PageSummaryDto[] = [];

  pageId: string | null = null;
  title = '';
  slug = '';
  meta: MetaDataDto = {};
  blocks: BlockDto[] = [];

  constructor() { this.loadPages(); }

  loadPages() { this.http.get<PageSummaryDto[]>('/api/pages').subscribe(x => this.pages = x); }
  deletePage(id: string) { this.http.delete(`/api/pages/${id}`).subscribe(() => this.loadPages()); }

  addBlock(type: string) {
    this.blocks.push({ id: crypto.randomUUID(), sortOrder: this.blocks.length, type, heading: `${type} heading`, body: '' });
  }

  reorder(event: CdkDragDrop<BlockDto[]>) {
    moveItemInArray(this.blocks, event.previousIndex, event.currentIndex);
    this.blocks = this.blocks.map((x, idx) => ({ ...x, sortOrder: idx }));
  }

  edit(page: PageSummaryDto) {
    this.pageId = page.id;
    this.http.get<any>(`/api/pages/${page.id}`).subscribe(p => {
      this.title = p.title;
      this.slug = p.slug;
      this.meta = p.metaData;
      this.blocks = p.blocks;
    });
  }

  createNew() {
    this.pageId = null;
    this.title = '';
    this.slug = '';
    this.meta = {};
    this.blocks = [];
  }

  save() {
    const payload = { title: this.title, slug: this.slug, metaData: this.meta, blocks: this.blocks };
    const request = this.pageId
      ? this.http.put(`/api/pages/${this.pageId}`, payload)
      : this.http.post('/api/pages', payload);
    request.subscribe(() => { this.createNew(); this.loadPages(); });
  }
}
