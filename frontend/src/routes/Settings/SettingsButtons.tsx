import { SpeedDial, SpeedDialAction } from '@mui/material';

import AddIcon from '@mui/icons-material/Add';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import UpdateIcon from '@mui/icons-material/Update';
import CleaningServicesIcon from '@mui/icons-material/CleaningServices';
import DeleteOutlineIcon from '@mui/icons-material/DeleteOutline';
import PublishedWithChangesIcon from '@mui/icons-material/PublishedWithChanges';
import { useAutoCompleteLibraryMutation } from '../../redux';

interface PropType {
  onAddDirectory: () => void;
  onAutoComplete: () => void;
  onLoadVideos: () => void;
  onCleanVideos: () => void;
  onRemoveDirectory: () => void;
}

const SettingsButtons = (props: PropType) => {
  const { onLoadVideos, onCleanVideos, onRemoveDirectory, onAutoComplete } = props;


  const handleLoadVideos = () => {
    onLoadVideos();
  };

  const handleCleanVideos = () => {
    onCleanVideos();
  };

  const handleAddDirectory = () => {
    props.onAddDirectory();
  };

  const handleRemoveDirectory = () => {
    onRemoveDirectory();
  };

  const handleAutoComplete = () => {
    onAutoComplete();
  };

  return (
    <SpeedDial
      icon={<MoreVertIcon />}
      ariaLabel=""
      direction="left"
      sx={{
        position: 'absolute',
        right: (theme) => theme.spacing(2),
        bottom: (theme) => theme.spacing(2)
      }}
      FabProps={{ size: 'small' }}
    >
      <SpeedDialAction
        icon={<UpdateIcon />}
        tooltipTitle="Load new videos"
        onClick={handleLoadVideos}
      ></SpeedDialAction>
      <SpeedDialAction
        icon={<CleaningServicesIcon />}
        tooltipTitle="Clean deleted videos"
        onClick={handleCleanVideos}
      ></SpeedDialAction>
      <SpeedDialAction
        icon={<AddIcon />}
        tooltipTitle="Add new directory"
        onClick={handleAddDirectory}
      ></SpeedDialAction>
      <SpeedDialAction
        icon={<DeleteOutlineIcon />}
        tooltipTitle="Remove selected directory"
        onClick={handleRemoveDirectory}
      ></SpeedDialAction>
      <SpeedDialAction
        icon={<PublishedWithChangesIcon />}
        tooltipTitle="Auto-complete library"
        onClick={handleAutoComplete}
      ></SpeedDialAction>
    </SpeedDial>
  );
};

export default SettingsButtons;
