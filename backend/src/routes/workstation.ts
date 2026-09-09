import { Router } from 'express';
import * as controller from '../controllers/workstationController';
import { authenticate } from '../middleware/authMiddleware';

const router = Router();

router.get('/', controller.listWorkstations);
router.post('/', authenticate, controller.createWorkstation);
router.get('/:id', controller.getWorkstation);
router.put('/:id', authenticate, controller.updateWorkstation);
router.delete('/:id', authenticate, controller.deleteWorkstation);

export default router;
