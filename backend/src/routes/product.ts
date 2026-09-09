import { Router } from 'express';
import * as controller from '../controllers/productController';

const router = Router();

router.get('/', controller.listProducts);
router.post('/', controller.createProduct);
router.get('/:id', controller.getProduct);
router.put('/:id', controller.updateProduct);
router.delete('/:id', controller.deleteProduct);

export default router;
