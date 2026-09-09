import { Router } from 'express';
import * as controller from '../controllers/assemblyLineController';

const router = Router();

router.get('/', controller.listAssemblyLines);
router.post('/', controller.createAssemblyLine);
router.get('/:id', controller.getAssemblyLine);
router.put('/:id', controller.updateAssemblyLine);
router.delete('/:id', controller.deleteAssemblyLine);

export default router;
