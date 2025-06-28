import LoadingButton from '@mui/lab/LoadingButton';
import { TextField } from '@mui/material';
import { useState } from 'react';

import { Video } from '../../models/Video';
import { useCreateContactSheetMutation } from '../../redux';

interface PropType {
  id: number;
  video?: Video;
}

const ContactSheetForm = (props: PropType) => {
  const { id } = props;
  const [nRows, setNRows] = useState(3);
  const [nCols, setNCols] = useState(4);
  const [createCs, status] = useCreateContactSheetMutation();

  const handleClick = () => {
    createCs({ videoId: id, nRows, nCols });
  };

  return (
    <form style={{ display: 'flex', alignItems: 'center' }}>
      <TextField
        label="Number of rows"
        value={nRows}
        onChange={(e) => setNRows(+e.target?.value)}
        type="number"
        margin="dense"
        variant="outlined"
        size="small"
        sx={{ flexGrow: 1 }}
        disabled={status.isLoading}></TextField>
      <TextField
        label="Number of columns"
        value={nCols}
        onChange={(e) => setNCols(+e.target?.value)}
        type="number"
        margin="dense"
        variant="outlined"
        size="small"
        sx={{ flexGrow: 1 }}
        disabled={status.isLoading}></TextField>
      <LoadingButton onClick={handleClick} variant="outlined" sx={{ m: 2 }} disabled={status.isLoading} loading={status.isLoading}>
        Generate
      </LoadingButton>
    </form>
  );
};

export default ContactSheetForm;
