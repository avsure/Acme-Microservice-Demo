import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService, Product } from '../../services/product.service';


@Component({
  standalone: true,
  imports: [CommonModule],
  selector: 'app-products',
  templateUrl: './products.component.html'
})

export class ProductsComponent implements OnInit {
  products: Product[] = [];
  loading = true;

  constructor(private productService: ProductService) {}

  ngOnInit() {
    this.productService.getAll().subscribe({
      next: (p: Product[]) => { 
      console.log("Products received", p);
      this.products = p; 
      this.loading = false; 
      },
      error: (err) => { 
        console.error('Error fetching products', err);
        this.loading = false; }
    });
  }

   // optional trackBy for ngFor performance
  trackById(index: number, item: Product) {
    return item?.id;
  }
}
