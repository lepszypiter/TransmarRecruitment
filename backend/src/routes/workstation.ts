import { Router } from 'express';
import * as controller from '../controllers/workstationController';

const router = Router();

router.get('/', controller.listWorkstations);
router.post('/', controller.createWorkstation);
router.get('/:id', controller.getWorkstation);
router.put('/:id', controller.updateWorkstation);
router.delete('/:id', controller.deleteWorkstation);

export default router;
